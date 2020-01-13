using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
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
