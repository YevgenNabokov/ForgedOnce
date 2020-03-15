using ForgedOnce.Core;
using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeFileLocationSelectors
{
    public class FileSystemLocationSelector : ICodeFileLocationSelector<CodeFileLocation>
    {
        private readonly string[] paths;
        private readonly IFileSystem fileSystem;
        private readonly string basePath;
        private readonly ICodeFileLocationFilter<CodeFileLocation> filter;

        public FileSystemLocationSelector(string[] paths, IFileSystem fileSystem, string basePath, ICodeFileLocationFilter<CodeFileLocation> filter = null)
        {
            this.paths = paths;
            this.fileSystem = fileSystem;
            this.basePath = basePath;
            this.filter = filter;
        }

        public IEnumerable<CodeFileLocation> GetLocations()
        {
            List<CodeFileLocation> result = new List<CodeFileLocation>();

            foreach (var path in this.paths)
            {
                var startDirectory = this.basePath;
                if (this.fileSystem.Path.IsPathRooted(path))
                {
                    startDirectory = fileSystem.Path.GetDirectoryName(path);
                }
                else
                {
                    foreach (var part in path.Split(this.fileSystem.Path.DirectorySeparatorChar))
                    {
                        if (part == "..")
                        {
                            startDirectory = this.fileSystem.Directory.GetParent(startDirectory).FullName;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                foreach (var file in this.fileSystem.Directory.GetFiles(startDirectory, "*.*", System.IO.SearchOption.AllDirectories))
                {
                    var location = new CodeFileLocation()
                    {
                        FilePath = file
                    };

                    if (this.filter == null || this.filter.IsMatch(location))
                    {
                        result.Add(location);
                    }
                }
            }

            return result;
        }
    }
}
