using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceCodeFileLocationProvider : ICodeFileLocationProvider
    {
        private readonly IWorkspaceManagerBase workspaceManager;
        private readonly string path;

        public WorkspaceCodeFileLocationProvider(IWorkspaceManagerBase workspaceManager, string path)
        {
            this.workspaceManager = workspaceManager;
            this.path = path;
        }

        public CodeFileLocation GetLocation(string name)
        {
            var pathParts = this.path.Split('/');
            var projName = pathParts[0];

            if (!this.workspaceManager.ProjectExists(projName))
            {
                throw new InvalidOperationException($"Cannot resolve project for path {this.path}");
            }

            return new WorkspaceCodeFileLocation()
            {
                DocumentPath = new DocumentPath(projName, pathParts.Skip(1), name)
            };
        }
    }
}
