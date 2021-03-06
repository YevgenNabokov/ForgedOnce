﻿using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeAnalysis
{
    public class WorkspaceManager : IWorkspaceManager
    {
        private Microsoft.CodeAnalysis.Workspace workspace;

        private WorkspaceFileStorageHandler workspaceFileStorageHandler;

        public WorkspaceManager(Microsoft.CodeAnalysis.Workspace workspace)
        {
            this.workspace = workspace;
            this.workspaceFileStorageHandler = new WorkspaceFileStorageHandler(this);
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

        public Document FindDocument(DocumentPath documentPath)
        {
            return this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == documentPath.ProjectName)?.Documents.FirstOrDefault(d => d.Folders.SequenceEqual(documentPath.ProjectFolders) && d.Name == documentPath.DocumentName);
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

        public Document FindDocumentByDocumentPath(DocumentPath documentPath)
        {
            var project = this.FindProject(documentPath.ProjectName);
            if (project != null)
            {
                foreach (var document in project.Documents)
                {
                    if (documentPath.ProjectFolders.Length == document.Folders.Count && documentPath.ProjectFolders.SequenceEqual(document.Folders))
                    {
                        if (document.Name == documentPath.DocumentName)
                        {
                            return document;
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<WorkspaceCodeFileLocation> CodeFileLocations
        {
            get
            {
                List<WorkspaceCodeFileLocation> result = new List<WorkspaceCodeFileLocation>();
                foreach (var project in this.workspace.CurrentSolution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        result.Add(
                            new WorkspaceCodeFileLocation()
                            {
                                DocumentPath = new DocumentPath(project.Name, document.Folders, document.Name),
                                FilePath = document.FilePath
                            });
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

        public Document AddCodeFile(DocumentPath documentPath, string sourceCodeText, string filePath = null)
        {
            var project = this.FindProject(documentPath.ProjectName);
            if (project == null)
            {
                throw new InvalidOperationException($"Project with Name={documentPath.ProjectName} is not found.");
            }
            
            var document = project.AddDocument(documentPath.DocumentName, sourceCodeText, documentPath.ProjectFolders, filePath);            
            
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

        public void ReplaceDocumentText(DocumentPath documentPath, string newText)
        {
            var document = this.FindDocument(documentPath);
            if (document == null)
            {
                throw new InvalidOperationException($"Document {documentPath} is not found in workspace.");
            }            

            var changed = document.WithText(SourceText.From(newText));
            this.ApplyChanges(changed.Project.Solution);
        }

        public void RemoveCodeFile(DocumentPath documentPath)
        {
            var document = this.FindDocument(documentPath);
            if (document == null)
            {
                throw new InvalidOperationException($"No document found to remove. DocumentName={documentPath}");
            }

            this.ApplyChanges(this.workspace.CurrentSolution.RemoveDocument(document.Id));
        }

        public void RemoveCodeFile(CodeFile codeFile)
        {
            if (codeFile.Location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = codeFile.Location as WorkspaceCodeFileLocation;
                if (workspaceLocation.DocumentPath != null)
                {
                    this.RemoveCodeFile(workspaceLocation.DocumentPath);
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
                    metadataReferences: proj.MetadataReferences,
                    compilationOptions: proj.CompilationOptions));

                foreach (var doc in proj.Documents)
                {
                    var cloneProj = solution.GetProject(proj.Id);
                    var sourceText = doc.GetTextAsync().Result;
                    var text = sourceText.ToString();
                    var newText = SourceText.From(text, sourceText.Encoding);
                    var document = cloneProj.AddDocument(doc.Name, newText, doc.Folders);
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

        public bool ProjectExists(string projectName)
        {
            return this.FindProject(projectName) != null;
        }

        public bool DocumentExists(string fullPath)
        {
            return this.FindDocumentByFilePath(fullPath) != null;
        }

        public void Add(CodeFile codeFile)
        {
            this.workspaceFileStorageHandler.Add(codeFile);
        }

        public void Remove(CodeFile codeFile)
        {
            this.workspaceFileStorageHandler.Remove(codeFile);
        }

        public bool CanResolveCodeFileName(CodeFileLocation location)
        {
            return this.workspaceFileStorageHandler.CanResolveCodeFileName(location);
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            return this.workspaceFileStorageHandler.ResolveCodeFileName(location);
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            this.workspaceFileStorageHandler.ResolveCodeFile(codeFile, resolveSourceCodeText, resolveLocation);
        }
    }
}
