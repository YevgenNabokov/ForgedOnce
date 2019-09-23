using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class DocumentSelector : IDocumentSelector
    {
        private readonly string[] documentPaths;

        public DocumentSelector(string[] documentPaths)
        {
            this.documentPaths = documentPaths;
        }

        public IEnumerable<WorkspaceCodeFileLocation> GetDocuments(IWorkspaceManagerBase workspaceManager)
        {
            List<WorkspaceCodeFileLocation> result = new List<WorkspaceCodeFileLocation>();
            foreach (var docPath in workspaceManager.DocumentPaths)
            {
                if (this.documentPaths.Any(m => PathMaskHelper.PathMatchMask(docPath.ToString(), m)))
                {
                    result.Add(workspaceManager.GetDocumentLocationByPath(docPath));
                }
                
            }

            return result;
        }
    }
}
