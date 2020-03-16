using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Logging;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Pipeline;
using ForgedOnce.Environment.Configuration;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Environment.Workspace.CodeFileLocationFilters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Builders
{
    public class PipelineBuilder : IBuilder<ICodeGenerationPipeline>
    {
        private readonly IBuilderProvider builderProvider;

        private readonly IPipelineWorkspaceManagers workspaceManagers;

        private readonly string basePath;
        private readonly IFileSystem fileSystem;
        private readonly ITypeLoader typeLoader;
        private readonly ILogger logger;

        public string Name => "GenericPipelineBuilder";

        public PipelineBuilder(
            IBuilderProvider builderProvider,
            IPipelineWorkspaceManagers workspaceManagers,
            string basePath,
            IFileSystem fileSystem,
            ITypeLoader typeLoader,
            ILogger logger)
        {
            this.builderProvider = builderProvider;
            this.workspaceManagers = workspaceManagers;
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
                    var inputBuilder = new CodeStreamProviderBuilder(result.PipelineEnvironment, this.workspaceManagers.OutputWorkspace, this.fileSystem, this.basePath);
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
                    Stages = stages,
                    Shadow = config.ShadowFilters.Select(c => this.BuildShadowFilter(c)).ToList(),
                    Unshadow = config.UnshadowFilters.Select(c => this.BuildShadowFilter(c)).ToList(),
                });
                index++;
            }

            return result;
        }

        private BatchShadowFilter BuildShadowFilter(CodeFileFilterRegistration config)
        {
            switch (config.Type)
            {
                case CodeFileFilterType.FileSystem:
                    return new BatchShadowFilter()
                    {
                        Language = config.Language,
                        Filter = new CodeFileLocationFilter(
                            this.fileSystem,
                            this.basePath,
                            config.Paths)
                    };
                case CodeFileFilterType.Project:
                    return new BatchShadowFilter()
                    {
                        Language = config.Language,
                        Filter = new WorkspaceCodeFileLocationFilter(config.Paths)
                    };
                default: throw new InvalidOperationException($"Shadow filter type not supported {config.Type}.");
            }
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

                var codeFileHandlerFactory = Activator.CreateInstance(type) as ICodeFileEnvironmentHandlerFactory;
                if (codeFileHandlerFactory == null)
                {
                    throw new InvalidOperationException($"CodeFileHandler type {handlerRegistration.Type} should implement {typeof(ICodeFileEnvironmentHandlerFactory)} interface.");
                }

                var handler = codeFileHandlerFactory.Create(this.workspaceManagers.ProcessingWorkspace, this.fileSystem, pipelineExecutionInfo, this.logger, handlerRegistration.Configuration);
                result.Handlers.Add(handler);
            }

            return result;
        }
    }
}
