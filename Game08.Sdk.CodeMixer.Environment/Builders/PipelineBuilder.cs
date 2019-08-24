using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Pipeline;
using Game08.Sdk.CodeMixer.Environment.Configuration;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class PipelineBuilder : IBuilder<ICodeGenerationPipeline>
    {
        private readonly IBuilderProvider builderProvider;

        private readonly IWorkspaceManager workspaceManager;

        private readonly string basePath;
        private readonly IFileSystem fileSystem;

        public string Name => "GenericPipelineBuilder";

        public PipelineBuilder(IBuilderProvider builderProvider, IWorkspaceManager workspaceManager, string basePath, IFileSystem fileSystem)
        {
            this.builderProvider = builderProvider;
            this.workspaceManager = workspaceManager;
            this.basePath = basePath;
            this.fileSystem = fileSystem;
        }

        public ICodeGenerationPipeline Build(JObject configuration)
        {
            var reader = new PipelineConfiguration(configuration);
            var result = new CodeGenerationPipeline();
            result.PipelineEnvironment = this.CreatePipelineEnvironment(reader);

            var inputConfig = reader.InputCodeStreamProviderConfiguration;
            if (inputConfig != null)
            {
                var inputBuilder = new CodeStreamProviderBuilder(result.PipelineEnvironment, this.workspaceManager, this.fileSystem, this.basePath);
                result.InputCodeStreamProvider = inputBuilder.Build(inputConfig);
            }

            result.Batches = this.BuildBatches(reader.BatchConfigurations);

            throw new NotImplementedException();

            return result;
        }

        private List<Batch> BuildBatches(IEnumerable<BatchConfiguration> batchConfigurations)
        {
            List<Batch> result = new List<Batch>();
            var stageBuilder = new StageBuilder(this.builderProvider);

            int index = 0;
            foreach (var config in batchConfigurations)
            {
                List<StageContainer> stages = new List<StageContainer>();

                foreach (var stage in config.Stages)
                {
                    stages.Add(stageBuilder.Build(stage));
                }

                result.Add(new Batch()
                {
                    Index = index,
                    Name = config.Name,
                    Stages = stages
                });
                index++;
            }

            return result;
        }

        private IPipelineEnvironment CreatePipelineEnvironment(PipelineConfiguration configuration)
        {
            var result = new PipelineEnvironment();

            foreach (var handlerRegistration in configuration.CodeFileHandlerTypeRegistrations)
            {
                Type type = Type.GetType(handlerRegistration.Type);
                if (type is null)
                {
                    throw new InvalidOperationException($"Cannot resolve CodeFileHandler type {handlerRegistration.Type}.");
                }

                var codeFileHandlerFactory = Activator.CreateInstance(type) as ICodeFileHandlerFactory;
                if (codeFileHandlerFactory == null)
                {
                    throw new InvalidOperationException($"CodeFileHandler type {handlerRegistration.Type} should implement {typeof(ICodeFileHandlerFactory)} interface.");
                }

                var handler = codeFileHandlerFactory.Create(this.workspaceManager, handlerRegistration.Configuration);
                result.Handlers.Add(handler);
            }

            return result;
        }
    }
}
