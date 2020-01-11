using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class PipelineWorkspaceManagersWrapper : IPipelineWorkspaceManagers
    {
        public PipelineWorkspaceManagersWrapper(IWorkspaceManager initialWorkspace, IWorkspaceManager processingWorkspace, IWorkspaceManagerBase outputWorkspace)
        {
            this.InitialWorkspace = initialWorkspace;
            this.ProcessingWorkspace = processingWorkspace;
            this.OutputWorkspace = outputWorkspace;
        }

        public IWorkspaceManager InitialWorkspace { get; private set; }

        public IWorkspaceManager ProcessingWorkspace { get; private set; }

        public IWorkspaceManagerBase OutputWorkspace { get; private set; }
    }
}
