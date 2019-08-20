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

        public Document FindDocumentByFilePath(string filePath)
        {
            var docs = this.workspace.CurrentSolution.GetDocumentIdsWithFilePath(filePath);

            if (docs.Length > 0)
            {
                return this.workspace.CurrentSolution.GetDocument(docs[0]);
            }

            return null;
        }

        public Document AddCodeFile(Guid projectId, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null)
        {
            var project = this.workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Id.Id == projectId);
            if (project == null)
            {
                throw new InvalidOperationException($"Project with Id={projectId} is not found.");
            }

            var proj = this.workspace.CurrentSolution.GetProject(project.Id);
            var document = proj.AddDocument(name, SourceText.From(sourceCodeText), projectFolders, filePath);
            return document;
        }

        public void RemoveCodeFile(Guid documentId)
        {
            var document = this.workspace.CurrentSolution.Projects.SelectMany(p => p.Documents).FirstOrDefault(d => d.Id.Id == documentId);
            if (document == null)
            {
                throw new InvalidOperationException($"No document found to remove. DocumentId={documentId}");
            }

            if (!this.workspace.TryApplyChanges(this.workspace.CurrentSolution.RemoveDocument(document.Id)))
            {
                throw new InvalidOperationException($"Failed to apply changes to solution after document removal.");
            }
        }

        public void RemoveCodeFile(CodeFile codeFile)
        {
            throw new NotImplementedException();
        }

        public IWorkspaceManager CreateAdHocClone()
        {
            throw new NotImplementedException();
        }
    }
}
