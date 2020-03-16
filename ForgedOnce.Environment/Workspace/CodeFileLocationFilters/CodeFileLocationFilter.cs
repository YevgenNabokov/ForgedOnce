using ForgedOnce.Core;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeFileLocationFilters
{
    public class CodeFileLocationFilter : ICodeFileLocationFilter
    {
        private readonly IFileSystem fileSystem;
        private readonly string basePath;
        private readonly string[] pathMasks;

        public CodeFileLocationFilter(IFileSystem fileSystem, string basePath, string[] pathMasks)
        {
            this.fileSystem = fileSystem;
            this.basePath = basePath;
            this.pathMasks = pathMasks;
        }

        public bool IsMatch(CodeFileLocation location)
        {
            foreach (var mask in this.pathMasks)
            {
                var startDirectoryMask = PathMaskHelper.GetAbsolutePathMask(mask, this.basePath, this.fileSystem);
                if (PathMaskHelper.PathMatchMask(location.FilePath, startDirectoryMask))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
