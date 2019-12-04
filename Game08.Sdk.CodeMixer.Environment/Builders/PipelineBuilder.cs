using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Logging;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
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

        private readonly IWorkspaceManagerBase initialWorkspaceManager;

        private readonly string basePath;
        private readonly IFileSystem fileSystem;
        private readonly ITypeLoader typeLoader;
        private readonly ILogger logger;

        public string Name => "GenericPipelineBuilder";

        public PipelineBuilder(
            IBuilderProvider builderProvider,
            IWorkspaceManager workspaceManager,
            IWorkspaceManagerBase initialWorkspaceManager,
            string basePath,
            IFileSystem fileSystem,
            ITypeLoader typeLoader,
            ILogger logger)
        {
            this.builderProvider = builderProvider;
            this.workspaceManager = workspaceManager;
            this.initialWorkspaceManager = initialWorkspaceManager;
            this.basePath = basePath;
            this.fileSystem = fileSystem;
            this.typeLoader = typeLoader;
            this.logger = logger;
        }

        public ICodeGenerationPipeline Build(JObject configuration)
        {
            this.logger.Write(new StageTopLevelInfoRecord("Building code generation pipeline."));

            try
            {
                var reader = new PipelineConfiguration(configuration);
                var result = new CodeGenerationPipeline(this.logger);
                result.PipelineEnvironment = this.CreatePipelineEnvironment(result.PipelineExecutionInfo, reader);

                var inputConfig = reader.InputCodeStreamProviderConfiguration;
                if (inputConfig != null)
                {
                    var inputBuilder = new CodeStreamProviderBuilder(result.PipelineEnvironment, this.initialWorkspaceManager, this.fileSystem, this.basePath);
                    result.InputCodeStreamProvider = inputBuilder.Build(inputConfig);
                }

                result.Batches = this.BuildBatches(reader.BatchConfigurations);

                return result;
            }
            catch (Exception ex)
            {
                this.logger.Write(new ErrorLogRecord("Error occurred while building code generation pipeline.", ex));
                throw;
            }
        }

        private List<Batch> BuildBatches(IEnumerable<BatchConfiguration> batchConfigurations)
        {
            List<Batch> result = new List<Batch>();
            var stageBuilder = new StageBuilder(this.builderProvider, this.typeLoader, this.logger);

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
                    PersistInputCodeStreams = new List<string>(config.PersistCodeInputStreams),
                    Stages = stages
                });
                index++;
            }

            return result;
        }

        private IPipelineEnvironment CreatePipelineEnvironment(IPipelineExecutionInfo pipelineExecutionInfo, PipelineConfiguration configuration)
        {
            var result = new PipelineEnvironment(pipelineExecutionInfo);

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

                var handler = codeFileHandlerFactory.Create(this.workspaceManager, this.fileSystem, pipelineExecutionInfo, handlerRegistration.Configuration);
                result.Handlers.Add(handler);
            }

            return result;
        }
    }
}
