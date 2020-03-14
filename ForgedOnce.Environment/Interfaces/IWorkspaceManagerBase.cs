using ForgedOnce.Environment.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IWorkspaceManagerBase : ICodeFileStorageHandler
    {
        IEnumerable<WorkspaceCodeFileLocation> CodeFileLocations
        {
            get;
        }

        bool ProjectExists(string projectName);

        bool DocumentExists(string fullPath);

        void RemoveCodeFile(DocumentPath documentPath);
    }
}
