using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeFileLocationSelectors
{
    public class WorkspaceLocationSelector : ICodeFileLocationSelector<WorkspaceCodeFileLocation>
    {
        private readonly IWorkspaceManagerBase workspaceManager;
        private readonly ICodeFileLocationFilter<WorkspaceCodeFileLocation> filter;

        public WorkspaceLocationSelector(IWorkspaceManagerBase workspaceManager, ICodeFileLocationFilter<WorkspaceCodeFileLocation> filter = null)
        {
            this.workspaceManager = workspaceManager;
            this.filter = filter;
        }

        public IEnumerable<WorkspaceCodeFileLocation> GetLocations()
        {
            List<WorkspaceCodeFileLocation> result = new List<WorkspaceCodeFileLocation>();
            foreach (var location in this.workspaceManager.CodeFileLocations)
            {
                if (this.filter == null || this.filter.IsMatch(location))
                {
                    result.Add(location);
                }
                
            }

            return result;
        }
    }
}
