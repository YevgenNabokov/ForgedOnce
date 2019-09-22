using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
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
