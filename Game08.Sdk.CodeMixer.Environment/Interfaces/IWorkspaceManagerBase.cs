using Game08.Sdk.CodeMixer.Environment.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IWorkspaceManagerBase
    {
        IEnumerable<DocumentPath> DocumentPaths
        {
            get;
        }

        WorkspaceCodeFileLocation GetDocumentLocationByPath(DocumentPath documentPath);

        bool ProjectExists(string projectName);

        bool DocumentExists(string fullPath);
    }
}
