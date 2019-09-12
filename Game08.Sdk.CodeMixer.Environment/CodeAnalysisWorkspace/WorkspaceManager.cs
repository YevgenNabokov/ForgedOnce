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

        public IEnumerable<TReference> GetMetadataReferences<TReference>(Guid? projectId = null) where TReference : MetadataReference
        {
            List<TReference> result = new List<TReference>();

            foreach (var proj in this.workspace.CurrentSolution.Projects)
            {
                if (projectId == null || proj.Id.Id == projectId.Value)
                {
                    foreach (var reference in proj.MetadataReferences.OfType<TReference>())
                    {
                        result.Add(reference);
                    }
                }
            }

            return result;
        }

        public IEnumerable<List<string>> GetProjectsDependencyChains(IEnumerable<string> projectNames)
        {
            List<List<string>> result = new List<List<string>>();

            var graph = this.workspace.CurrentSolution.GetProjectDependencyGraph();

            foreach (var name in projectNames)
            {
                var projId = this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == name).Id;
                var chain = graph.GetProjectsThatThisProjectTransitivelyDependsOn(projId).Select(p => this.workspace.CurrentSolution.GetProject(p).Name).ToList();
                chain.Add(name);
                result.Add(chain);
            }

            return result;
        }

        public Document FindDocument(string projectName, string[] projectFolders, string documentName)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == projectName)?.Documents.FirstOrDefault(d => d.Folders.SequenceEqual(projectFolders) && d.Name == documentName);
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
                        result.Add(DocumentPathHelper.GetPath(project.Name, document.Folders, document.Name));
                    }
                }

                return result;
            }
        }

        public Project FindProjectByAssemblyName(string assemblyName)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.AssemblyName == assemblyName);
        }

        public Project FindProject(string projectName)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == projectName);
        }

        public Project FindProject(Guid id)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Id.Id == id);
        }

        public Document AddCodeFile(string projectName, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null)
        {
            var project = this.FindProject(projectName);
            if (project == null)
            {
                throw new InvalidOperationException($"Project with Name={projectName} is not found.");
            }
            
            var document = project.AddDocument(name, sourceCodeText, projectFolders, filePath);            
            
            this.ApplyChanges(document.Project.Solution);

            return document;
        }

        public void ReplaceDocumentText(Guid documentId, string newText)
        {
            var document = this.workspace.CurrentSolution.Projects.SelectMany(p => p.Documents).FirstOrDefault(d => d.Id.Id == documentId);
            if (document == null)
            {
                throw new InvalidOperationException($"Document {documentId} is not found in workspace.");
            }

            var changed = document.WithText(SourceText.From(newText));
            this.ApplyChanges(changed.Project.Solution);
        }

        public void ReplaceDocumentText(string projectName, string[] projectFolders, string documentName, string newText)
        {
            var document = this.FindDocument(projectName, projectFolders, documentName);
            if (document == null)
            {
                throw new InvalidOperationException($"Document {documentName} is not found in workspace.");
            }            

            var changed = document.WithText(SourceText.From(newText));
            this.ApplyChanges(changed.Project.Solution);
        }

        public void RemoveCodeFile(string projectName, string[] projectFolders, string documentName)
        {
            var document = this.FindDocument(projectName, projectFolders, documentName);
            if (document == null)
            {
                throw new InvalidOperationException($"No document found to remove. DocumentName={documentName}");
            }

            this.ApplyChanges(this.workspace.CurrentSolution.RemoveDocument(document.Id));
        }

        public void RemoveCodeFile(CodeFile codeFile)
        {
            if (codeFile.Location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = codeFile.Location as WorkspaceCodeFileLocation;
                if (!string.IsNullOrEmpty(workspaceLocation.DocumentName))
                {
                    this.RemoveCodeFile(workspaceLocation.ProjectName, workspaceLocation.ProjectFolders, workspaceLocation.DocumentName);
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
                    var text = doc.GetTextAsync().Result.ToString();
                    var newText = SourceText.From(text);
                    var document = cloneProj.AddDocument(doc.Name, text, doc.Folders);
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
