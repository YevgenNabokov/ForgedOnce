﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class WorkspaceCodeFileDestination : ICodeFileDestination
    {
        private readonly IPipelineWorkspaceManagers workspaces;
        private readonly string path;
        private readonly string projectName;
        private readonly string[] pathParts;

        public WorkspaceCodeFileDestination(IPipelineWorkspaceManagers workspaces, string path)
        {
            this.workspaces = workspaces;
            this.path = path;
            var parts = this.path.Split('/');
            this.pathParts = parts.Skip(1).ToArray();
            this.projectName = parts[0];
        }

        public CodeFileLocation GetLocation(string fileName)
        {
            if (!this.workspaces.OutputWorkspace.ProjectExists(this.projectName))
            {
                throw new InvalidOperationException($"Cannot resolve project for path {this.path}");
            }

            return new WorkspaceCodeFileLocation()
            {
                DocumentPath = new DocumentPath(this.projectName, this.pathParts, fileName)
            };
        }

        public void Clear()
        {
            this.ClearInWorkspace(this.workspaces.ProcessingWorkspace);
            this.ClearInWorkspace(this.workspaces.OutputWorkspace);
        }

        private void ClearInWorkspace(IWorkspaceManagerBase workspace)
        {
            foreach (var path in workspace.DocumentPaths)
            {
                if (path.ProjectName == this.projectName && path.ProjectFolders.SequenceEqual(this.pathParts))
                {
                    workspace.RemoveCodeFile(path);
                }
            }
        }
    }
}
