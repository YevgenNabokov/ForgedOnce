using ForgedOnce.Core;
using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace
{
    public class FileSelector : IFileSelector
    {
        private readonly string[] filePaths;

        public FileSelector(string[] filePaths)
        {
            this.filePaths = filePaths;
        }

        public IEnumerable<CodeFileLocation> GetFiles(IFileSystem fileSystem, string basePath)
        {
            List<CodeFileLocation> result = new List<CodeFileLocation>();

            foreach (var mask in this.filePaths)
            {
                var startDirectory = basePath;                        
                foreach (var part in mask.Split(fileSystem.Path.DirectorySeparatorChar))
                {
                    if (part == "..")
                    {
                        startDirectory = fileSystem.Directory.GetParent(startDirectory).FullName;                        
                    }
                    else
                    {
                        break;
                    }
                }

                var startDirectoryMask = PathMaskHelper.GetAbsolutePathMask(mask, basePath, fileSystem);

                foreach (var file in fileSystem.Directory.GetFiles(startDirectory, "*.*", System.IO.SearchOption.AllDirectories))
                {
                    if (PathMaskHelper.PathMatchMask(file, startDirectoryMask))
                    {
                        result.Add(new CodeFileLocation()
                        {
                            FilePath = file
                        });
                    }
                }
            }

            return result;
        }
    }
}
