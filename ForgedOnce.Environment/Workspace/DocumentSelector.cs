using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace
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
            foreach (var location in workspaceManager.CodeFileLocations)
            {
                if (this.documentPaths.Any(m => PathMaskHelper.PathMatchMask(location.ToString(), m)))
                {
                    result.Add(location);
                }
                
            }

            return result;
        }
    }
}
