using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IPipelineWorkspaceManagers
    {
        IWorkspaceManager InitialWorkspace { get; }

        IWorkspaceManager ProcessingWorkspace { get; }

        IWorkspaceManagerBase OutputWorkspace { get; }
    }
}
