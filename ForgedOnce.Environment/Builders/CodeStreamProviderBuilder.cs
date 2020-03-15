using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Configuration;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using ForgedOnce.Environment.Workspace.CodeFileLocationSelectors;
using ForgedOnce.Environment.Workspace.CodeFileLocationFilters;

namespace ForgedOnce.Environment.Builders
{
    public class CodeStreamProviderBuilder : IBuilder<ICodeStreamProvider>
    {
        private readonly IPipelineEnvironment pipelineEnvironment;
        private readonly IWorkspaceManagerBase workspaceManager;
        private readonly IFileSystem fileSystem;
        private readonly string basePath;

        public string Name => "GenericCodeStreamProvider";

        public CodeStreamProviderBuilder(IPipelineEnvironment pipelineEnvironment,
            IWorkspaceManagerBase workspaceManager,
            IFileSystem fileSystem,
            string basePath)
        {
            this.pipelineEnvironment = pipelineEnvironment;
            this.workspaceManager = workspaceManager;
            this.fileSystem = fileSystem;
            this.basePath = basePath;
        }

        public ICodeStreamProvider Build(JObject configuration)
        {
            List<ICodeStreamProvider> providers = new List<ICodeStreamProvider>();
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
                                this.pipelineEnvironment,
                                new FileSystemLocationSelector(
                                    provider.Paths,
                                    this.fileSystem,
                                    this.basePath,
                                    new CodeFileLocationFilter(
                                        this.fileSystem,
                                        this.basePath,
                                        provider.Paths))));
                        }; break;
                    case CodeStreamProviderType.Project:
                        {
                            providers.Add(new WorkspaceCodeStreamProvider(
                                provider.Language,
                                provider.Name,
                                this.pipelineEnvironment,
                                new WorkspaceLocationSelector(
                                    this.workspaceManager,
                                    new WorkspaceCodeFileLocationFilter(provider.Paths))));
                        }; break;
                    case CodeStreamProviderType.CreateNew:
                        {
                            providers.Add(new NewFileCodeStreamProvider(
                                provider.Language,
                                provider.Name,
                                provider.Paths,
                                this.pipelineEnvironment));
                        }; break;
                    default: throw new InvalidOperationException($"Code stream provider not supported {provider.Type}.");
                }
            }

            return new PipelineCodeStreamProvider(providers);
        }
    }
}
