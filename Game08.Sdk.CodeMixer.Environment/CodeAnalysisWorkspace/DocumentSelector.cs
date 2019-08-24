using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class DocumentSelector : IDocumentSelector
    {
        private readonly string[] documentPaths;

        public DocumentSelector(string[] documentPaths)
        {
            this.documentPaths = documentPaths;
        }

        public IEnumerable<Document> GetDocuments(IWorkspaceManager workspaceManager)
        {
            List<Document> result = new List<Document>();
            foreach (var docPath in workspaceManager.DocumentPaths)
            {
                if (this.documentPaths.Any(m => PathMaskHelper.PathMatchMask(docPath, m)))
                {
                    result.Add(workspaceManager.FindDocumentByDocumentPath(docPath));
                }
                
            }

            return result;
        }
    }
}
