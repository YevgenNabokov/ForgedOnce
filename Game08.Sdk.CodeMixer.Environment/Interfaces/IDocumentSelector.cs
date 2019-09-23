using Game08.Sdk.CodeMixer.Environment.Workspace;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IDocumentSelector
    {
        IEnumerable<WorkspaceCodeFileLocation> GetDocuments(IWorkspaceManagerBase workspaceManager);
    }
}
