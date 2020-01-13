using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class FileSystemCodeFileDestination : ICodeFileDestination
    {
        private readonly IFileSystem fileSystem;
        private readonly string basePath;
        private readonly string path;
        private string locationFullPath;

        public FileSystemCodeFileDestination(IFileSystem fileSystem, string basePath, string path)
        {
            this.fileSystem = fileSystem;
            this.basePath = basePath;
            this.path = path;
        }

        public void Clear()
        {
            foreach (var file in this.fileSystem.Directory.GetFiles(this.LocationFullPath))
            {
                this.fileSystem.File.Delete(file);
            }
        }

        public CodeFileLocation GetLocation(string fileName)
        {
            return new CodeFileLocation()
            {
                FilePath = this.fileSystem.Path.Combine(this.LocationFullPath, fileName)
            };
        }

        protected string LocationFullPath
        {
            get
            {
                if (this.locationFullPath == null)
                {
                    if (this.fileSystem.Path.IsPathRooted(this.path))
                    {
                        this.locationFullPath = this.fileSystem.Path.GetFullPath(this.path);
                    }
                    else
                    {
                        this.locationFullPath = this.fileSystem.Path.GetFullPath(this.fileSystem.Path.Combine(this.basePath, this.path));
                    }
                }

                return this.locationFullPath;
            }
        }
    }
}
