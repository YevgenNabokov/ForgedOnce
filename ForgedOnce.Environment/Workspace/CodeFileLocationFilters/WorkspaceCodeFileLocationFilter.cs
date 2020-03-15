using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeFileLocationFilters
{
    public class WorkspaceCodeFileLocationFilter : ICodeFileLocationFilter<WorkspaceCodeFileLocation>
    {
        private readonly string[] documentPaths;

        public WorkspaceCodeFileLocationFilter(string[] documentPaths)
        {
            this.documentPaths = documentPaths;
        }


        public bool IsMatch(WorkspaceCodeFileLocation location)
        {
            return this.documentPaths.Any(m => PathMaskHelper.PathMatchMask(location.ToString(), m));
        }
    }
}
