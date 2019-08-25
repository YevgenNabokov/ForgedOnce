using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceManager : IWorkspaceManager
    {
        private Workspace workspace;

        public WorkspaceManager(Workspace workspace)
        {
            this.workspace = workspace;
        }        

        public IEnumerable<List<Guid>> GetProjectsDependencyChains(IEnumerable<Guid> projectIds)
        {
            List<List<Guid>> result = new List<List<Guid>>();

            var graph = this.workspace.CurrentSolution.GetProjectDependencyGraph();

            foreach (var id in projectIds)
            {
                var projId = this.workspace.CurrentSolution.ProjectIds.FirstOrDefault(p => p.Id == id);
                result.Add(graph.GetProjectsThatThisProjectTransitivelyDependsOn(projId).Select(p => p.Id).ToList());
            }

            return result;
        }

        public Document FindDocument(Guid documentId)
        {
            return this.workspace.CurrentSolution.Projects.SelectMany(p => p.Documents).FirstOrDefault(d => d.Id.Id == documentId);
        }

        public Document FindDocumentByFilePath(string filePath)
        {
            var docs = this.workspace.CurrentSolution.GetDocumentIdsWithFilePath(filePath);

            if (docs.Length > 0)
            {
                return this.workspace.CurrentSolution.GetDocument(docs[0]);
            }

            return null;
        }

        public Document FindDocumentByDocumentPath(string documentPath)
        {
            var parts = documentPath.Split('/');
            var folders = new string[parts.Length - 2];
            Array.Copy(parts, 1, folders, 0, parts.Length - 2);
            var project = this.FindProject(parts[0]);
            if (project != null)
            {
                foreach (var document in project.Documents)
                {
                    if (folders.Length == document.Folders.Count && folders.SequenceEqual(document.Folders))
                    {
                        if (document.Name == parts.Last())
                        {
                            return document;
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<string> DocumentPaths
        {
            get
            {
                List<string> result = new List<string>();
                foreach (var project in this.workspace.CurrentSolution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        var folders = string.Join("/", document.Folders);
                        result.Add($"{project.Name}/{folders}/{document.Name}");
                    }
                }

                return result;
            }
        }

        public Project FindProject(string projectName)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == projectName);
        }

        public Project FindProject(Guid id)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Id.Id == id);
        }

        public Document AddCodeFile(Guid projectId, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null)
        {
            var project = this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Id.Id == projectId);
            if (project == null)
            {
                throw new InvalidOperationException($"Project with Id={projectId} is not found.");
            }
            
            var document = project.AddDocument(name, SourceText.From(sourceCodeText), projectFolders, filePath);

            this.ApplyChanges(document.Project.Solution);

            return document;
        }

        public void RemoveCodeFile(Guid documentId)
        {
            var document = this.workspace.CurrentSolution.Projects.SelectMany(p => p.Documents).FirstOrDefault(d => d.Id.Id == documentId);
            if (document == null)
            {
                throw new InvalidOperationException($"No document found to remove. DocumentId={documentId}");
            }

            this.ApplyChanges(this.workspace.CurrentSolution.RemoveDocument(document.Id));
        }

        public void RemoveCodeFile(CodeFile codeFile)
        {
            if (codeFile.Location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = codeFile.Location as WorkspaceCodeFileLocation;
                if (workspaceLocation.DocumentId != Guid.Empty)
                {
                    this.RemoveCodeFile(workspaceLocation.DocumentId);
                }
            }
            else
            {
                var document = this.FindDocumentByFilePath(codeFile.Location.FilePath);
                if (document != null)
                {
                    this.ApplyChanges(this.workspace.CurrentSolution.RemoveDocument(document.Id));
                }
            }
        }

        public IWorkspaceManager CreateAdHocClone()
        {
            var clone = new AdhocWorkspace();

            var solution = clone.AddSolution(SolutionInfo.Create(this.workspace.CurrentSolution.Id, this.workspace.CurrentSolution.Version));
            
            foreach (var proj in this.workspace.CurrentSolution.Projects)
            {                
                solution = solution.AddProject(ProjectInfo.Create(
                    proj.Id,
                    proj.Version,
                    proj.Name,
                    proj.AssemblyName,
                    proj.Language,
                    projectReferences: proj.ProjectReferences,
                    metadataReferences: proj.MetadataReferences));

                foreach (var doc in proj.Documents)
                {
                    var cloneProj = solution.GetProject(proj.Id);
                    var document = cloneProj.AddDocument(doc.Name, doc.GetTextAsync().Result, doc.Folders);
                    solution = document.Project.Solution;
                }
            }

            if (!clone.TryApplyChanges(solution))
            {
                throw new InvalidOperationException("Cannot apply changes to workspace during cloning.");
            }

            return new WorkspaceManager(clone);
        }

        private void ApplyChanges(Solution solution)
        {
            if (!this.workspace.TryApplyChanges(solution))
            {
                throw new InvalidOperationException($"Failed to apply changes to solution.");
            }
        }
    }
}
