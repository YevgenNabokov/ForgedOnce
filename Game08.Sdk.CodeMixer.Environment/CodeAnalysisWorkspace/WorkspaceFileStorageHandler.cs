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

            if (!string.IsNullOrEmpty(location.FilePath))
            {
                var existingDocument = this.workspaceManager.FindDocumentByFilePath(location.FilePath);
                if (existingDocument != null)
                {
                    this.workspaceManager.ReplaceDocumentText(existingDocument.Id.Id, codeFile.SourceCodeText);
                    location.DocumentPath = new DocumentPath(existingDocument.Project.Name, existingDocument.Folders, existingDocument.Name);
                    return;
                    ////this.workspaceManager.RemoveCodeFile(existingDocument.Id.Id);
                }
            }

            if (location.DocumentPath != null)
            {
                var existingDocument = this.workspaceManager.FindDocument(location.DocumentPath);
                if (existingDocument != null)
                {
                    this.workspaceManager.ReplaceDocumentText(existingDocument.Id.Id, codeFile.SourceCodeText);
                    location.DocumentPath = new DocumentPath(existingDocument.Project.Name, existingDocument.Folders, existingDocument.Name);
                    return;
                    ////this.workspaceManager.RemoveCodeFile(existingDocument.Id.Id);
                }
            }

            var document = this.workspaceManager.AddCodeFile(location.DocumentPath, codeFile.SourceCodeText, location.FilePath);
            location.DocumentPath = new DocumentPath(document.Project.Name, document.Folders, document.Name);
        }

        public void Remove(CodeFile codeFile)
        {
            if (codeFile.Language == null || !(codeFile.Location is WorkspaceCodeFileLocation))
            {
                throw new InvalidOperationException($"Cannot remove code file it should have {nameof(codeFile.Location)} as {nameof(WorkspaceCodeFileLocation)}.");
            }

            var location = codeFile.Location as WorkspaceCodeFileLocation;
            this.workspaceManager.RemoveCodeFile(location.DocumentPath);
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
                        location.DocumentPath = new DocumentPath(document.Project.Name, document.Folders.ToArray(), document.Name);

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
                if (workspaceLocation.DocumentPath != null)
                {
                    document = this.workspaceManager.FindDocument(workspaceLocation.DocumentPath);
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
