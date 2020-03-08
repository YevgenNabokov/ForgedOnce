using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Workspace
{
    public class PipelineWorkspaceManagersWrapper : IPipelineWorkspaceManagers
    {
        public PipelineWorkspaceManagersWrapper(IWorkspaceManager processingWorkspace, IWorkspaceManagerBase outputWorkspace)
        {
            this.ProcessingWorkspace = processingWorkspace;
            this.OutputWorkspace = outputWorkspace;
        }
        
        public IWorkspaceManager ProcessingWorkspace { get; private set; }

        public IWorkspaceManagerBase OutputWorkspace { get; private set; }
    }
}
