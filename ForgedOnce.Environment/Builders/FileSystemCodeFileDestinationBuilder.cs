using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Environment.Workspace;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment.Builders
{
    public class FileSystemCodeFileDestinationBuilder : IBuilder<ICodeFileDestination>
    {
        public const string PathKey = "path";
        private readonly IFileSystem fileSystem;
        private readonly string basePath;

        public string Name => null;

        public FileSystemCodeFileDestinationBuilder(IFileSystem fileSystem, string basePath)
        {
            this.fileSystem = fileSystem;
            this.basePath = basePath;
        }

        public ICodeFileDestination Build(JObject configuration)
        {
            if (!configuration.ContainsKey(PathKey))
            {
                throw new InvalidOperationException($"Settings for {nameof(FileSystemCodeFileDestinationBuilder)} should contain {PathKey}.");
            }

            var path = configuration[PathKey].Value<string>();

            return new FileSystemCodeFileDestination(this.fileSystem, this.basePath, path);
        }
    }
}
