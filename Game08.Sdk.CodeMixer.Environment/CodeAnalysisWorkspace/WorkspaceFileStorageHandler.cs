using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceFileStorageHandler : ICodeFileStorageHandler
    {
        private IWorkspaceManager workspaceManager;

        public WorkspaceFileStorageHandler(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public void Add(CodeFile codeFile)
        {            
            if (codeFile.Language == null || !(codeFile.Location is WorkspaceCodeFileLocation))
            {
                throw new InvalidOperationException($"Cannot add code file to {nameof(WorkspaceFileStorageHandler)} it should have {nameof(codeFile.Location)} as {nameof(WorkspaceCodeFileLocation)}.");
            }

            var location = codeFile.Location as WorkspaceCodeFileLocation;
            if (location.DocumentId != Guid.Empty && this.workspaceManager.FindDocument(location.DocumentId) != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(location.FilePath))
            {
                var existingDocument = this.workspaceManager.FindDocumentByFilePath(location.FilePath);
                if (existingDocument != null)
                {
                    this.workspaceManager.ReplaceDocumentText(existingDocument.Id.Id, codeFile.SourceCodeText);
                    location.DocumentId = existingDocument.Id.Id;
                    return;
                    ////this.workspaceManager.RemoveCodeFile(existingDocument.Id.Id);
                }
            }

            if (location.ProjectId != Guid.Empty)
            {
                var proj = this.workspaceManager.FindProject(location.ProjectId);
                if (proj != null)
                {
                    var existingDocument = this.workspaceManager.FindDocumentByDocumentPath(DocumentPathHelper.GetPath(proj.Name, location.ProjectFolders, codeFile.Name));
                    if (existingDocument != null)
                    {
                        this.workspaceManager.ReplaceDocumentText(existingDocument.Id.Id, codeFile.SourceCodeText);
                        location.DocumentId = existingDocument.Id.Id;
                        return;
                        ////this.workspaceManager.RemoveCodeFile(existingDocument.Id.Id);
                    }
                }
            }

            var document = this.workspaceManager.AddCodeFile(location.ProjectId, location.ProjectFolders, codeFile.Name, codeFile.SourceCodeText, location.FilePath);
            location.DocumentId = document.Id.Id;
        }

        public void Remove(CodeFile codeFile)
        {
            if (codeFile.Language == null || !(codeFile.Location is WorkspaceCodeFileLocation))
            {
                throw new InvalidOperationException($"Cannot remove code file it should have {nameof(codeFile.Location)} as {nameof(WorkspaceCodeFileLocation)}.");
            }

            var location = codeFile.Location as WorkspaceCodeFileLocation;
            this.workspaceManager.RemoveCodeFile(location.DocumentId);
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            var document = this.FindDocument(location);
            if (document != null)
            {
                return document.Name;
            }

            return null;
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            var document = this.FindDocument(codeFile.Location);

            if (document != null)
            {
                SourceText text = document.GetTextAsync().Result;
                if (text != null)
                {
                    if (resolveSourceCodeText)
                    {
                        codeFile.SourceCodeText = text.ToString();
                    }

                    if (resolveLocation)
                    {
                        var location = codeFile.Location is WorkspaceCodeFileLocation ? (WorkspaceCodeFileLocation)codeFile.Location : new WorkspaceCodeFileLocation(codeFile.Location);
                        location.DocumentId = document.Id.Id;
                        location.ProjectId = document.Project.Id.Id;
                        location.ProjectFolders = document.Folders.ToArray();

                        codeFile.Location = location;
                    }
                }
            }
        }

        private Document FindDocument(CodeFileLocation location)
        {
            Document document = null;

            if (location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = location as WorkspaceCodeFileLocation;
                if (workspaceLocation.DocumentId != Guid.Empty)
                {
                    document = this.workspaceManager.FindDocument(workspaceLocation.DocumentId);
                }
            }
            else
            {
                document = this.workspaceManager.FindDocumentByFilePath(location.FilePath);
            }

            return document;
        }
    }
}
