using ForgedOnce.Environment.Workspace;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IDocumentSelector
    {
        IEnumerable<WorkspaceCodeFileLocation> GetDocuments(IWorkspaceManagerBase workspaceManager);
    }
}
