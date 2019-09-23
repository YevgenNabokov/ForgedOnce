using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Configuration;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class CodeStreamProviderBuilder : IBuilder<ICodeStreamProvider>
    {
        private readonly ICodeFileResolver codeFileResolver;
        private readonly IWorkspaceManagerBase workspaceManager;
        private readonly IFileSystem fileSystem;
        private readonly string basePath;

        public string Name => "GenericCodeStreamProvider";

        public CodeStreamProviderBuilder(ICodeFileResolver codeFileResolver,
            IWorkspaceManagerBase workspaceManager,
            IFileSystem fileSystem,
            string basePath)
        {
            this.codeFileResolver = codeFileResolver;
            this.workspaceManager = workspaceManager;
            this.fileSystem = fileSystem;
            this.basePath = basePath;
        }

        public ICodeStreamProvider Build(JObject configuration)
        {
            List<WorkspaceCodeStreamProvider> providers = new List<WorkspaceCodeStreamProvider>();
            var configModel = new InputProviderConfiguration(configuration);

            foreach (var provider in configModel.ProviderRegistrations)
            {
                switch (provider.Type)
                {
                    case CodeStreamProviderType.FileSystem:
                        {
                            providers.Add(new WorkspaceCodeStreamProvider(
                                provider.Language,
                                provider.Name,
                                this.codeFileResolver,
                                this.workspaceManager,
                                this.fileSystem,
                                this.basePath,
                                new FileSelector(provider.Paths)));
                        }; break;
                    case CodeStreamProviderType.Project:
                        {
                            providers.Add(new WorkspaceCodeStreamProvider(
                                provider.Language,
                                provider.Name,
                                this.codeFileResolver,
                                this.workspaceManager,
                                new DocumentSelector(provider.Paths)));
                        }; break;
                    default: throw new InvalidOperationException($"Code stream provider not supported {provider.Type}.");
                }
            }

            return new PipelineCodeStreamProvider(providers);
        }
    }
}
