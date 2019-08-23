using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IDocumentSelector
    {
        IEnumerable<Document> GetDocuments(IWorkspaceManager workspaceManager);
    }
}
