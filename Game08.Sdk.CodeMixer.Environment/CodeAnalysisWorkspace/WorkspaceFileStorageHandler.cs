using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
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

        public void ResolveSourceCodeText(CodeFile codeFile)
        {
            var document = this.workspaceManager.FindDocumentByFilePath(codeFile.Location.FilePath);
            if (document != null)
            {
                SourceText text = null;
                if (document.TryGetText(out text))
                {
                    codeFile.SourceCodeText = text.ToString();
                    codeFile.Location = new WorkspaceCodeFileLocation(codeFile.Location) { DocumentId = document.Id.Id };
                }
            }
        }
    }
}
