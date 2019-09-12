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
        private readonly IWorkspaceManager workspaceManager;
        private readonly string path;

        public WorkspaceCodeFileLocationProvider(IWorkspaceManager workspaceManager, string path)
        {
            this.workspaceManager = workspaceManager;
            this.path = path;
        }

        public CodeFileLocation GetLocation(string name)
        {
            var pathParts = this.path.Split('/');
            var project = this.workspaceManager.FindProject(pathParts[0]);

            if (project == null)
            {
                throw new InvalidOperationException($"Cannot resolve project for path {this.path}");
            }

            return new WorkspaceCodeFileLocation()
            {
                ProjectName = project.Name,
                ProjectFolders = pathParts.Skip(1).ToArray(),
                DocumentName = name
            };
        }
    }
}
