using ForgedOnce.Core;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeFileLocationFilters
{
    public class WorkspaceCodeFileLocationFilter : ICodeFileLocationFilter
    {
        private readonly string[] documentPaths;

        public WorkspaceCodeFileLocationFilter(string[] documentPaths)
        {
            this.documentPaths = documentPaths;
        }


        public bool IsMatch(CodeFileLocation location)
        {
            if (location is WorkspaceCodeFileLocation workspaceCodeFileLocation)
            {
                return this.documentPaths.Any(m => PathMaskHelper.PathMatchMask(workspaceCodeFileLocation.DocumentPath.ToString(), m));
            }

            throw new NotSupportedException($"{nameof(WorkspaceCodeFileLocationFilter)} supports only locations of type: {typeof(WorkspaceCodeFileLocation)}");
        }
    }
}
