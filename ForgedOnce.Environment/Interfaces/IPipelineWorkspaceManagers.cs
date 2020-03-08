using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IPipelineWorkspaceManagers
    {
        IWorkspaceManager ProcessingWorkspace { get; }

        IWorkspaceManagerBase OutputWorkspace { get; }
    }
}
