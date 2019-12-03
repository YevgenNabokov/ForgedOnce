using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Builders;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders;
using Game08.Sdk.CodeMixer.Environment.Workspace.TypeLoaders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Environment
{
    public class CodeGenerationPipelineLauncher
    {
        private readonly IWorkspaceManager codeAnalysisWorkspaceManager;
        private readonly IWorkspaceManagerBase initialWorkspaceManager;
        private readonly IFileSystem fileSystem;
        private readonly ITypeLoader additionalTypeLoader;
        private readonly ICodeFileStorageHandler outputStorageHandler;

        public CodeGenerationPipelineLauncher(
            IWorkspaceManager codeAnalysisWorkspaceManager,
            IWorkspaceManagerBase initialWorkspaceManager,
            IFileSystem fileSystem,
            ITypeLoader additionalTypeLoader = null,
            ICodeFileStorageHandler outputStorageHandler = null)
        {
            this.codeAnalysisWorkspaceManager = codeAnalysisWorkspaceManager;
            this.initialWorkspaceManager = initialWorkspaceManager;
            this.fileSystem = fileSystem;
            this.additionalTypeLoader = additionalTypeLoader;
            this.outputStorageHandler = outputStorageHandler;
        }

        public void Launch(string pipelineConfigurationFilePath)
        {
            if (!fileSystem.File.Exists(pipelineConfigurationFilePath))
            {
                throw new InvalidOperationException($"Pipeline configuration file not found: {pipelineConfigurationFilePath}.");
            }            

            var typeLoader = new AggregateTypeLoader(
                new DefaultTypeLoader(),
                new ProjectReferenceTypeLoader(codeAnalysisWorkspaceManager, this.fileSystem),
                new WorkspaceTypeLoader(codeAnalysisWorkspaceManager));
            if (this.additionalTypeLoader != null)
            {
                typeLoader.AddResolver(this.additionalTypeLoader);
            }

            var trackingTypeLoader = new TrackingTypeLoaderWrapper(typeLoader, this.fileSystem);

            trackingTypeLoader.AttachAssemblyResolveHandler();

            var processWorkspaceManager = codeAnalysisWorkspaceManager.CreateAdHocClone();
            var basePath = this.fileSystem.Path.GetDirectoryName(this.fileSystem.Path.GetFullPath(pipelineConfigurationFilePath));
            var builderProvider = this.GetBuilderProvider(initialWorkspaceManager);

            var pipelineBuilder = new PipelineBuilder(
                builderProvider,
                processWorkspaceManager,
                this.initialWorkspaceManager,
                basePath,
                this.fileSystem,
                trackingTypeLoader);

            var pipeline = pipelineBuilder.Build(JObject.Parse(this.fileSystem.File.ReadAllText(pipelineConfigurationFilePath)));

            this.ExecutePipeline(pipeline);

            this.SaveOutputs(codeAnalysisWorkspaceManager, pipeline.GetOutputs());
        }

        private void ExecutePipeline(ICodeGenerationPipeline pipeline)
        {
            pipeline.Execute();
        }

        private void SaveOutputs(IWorkspaceManager workspaceManager, IEnumerable<CodeFile> outputs)
        {
            var handler = this.outputStorageHandler ?? new WorkspaceFileStorageHandler(workspaceManager);

            foreach (var file in outputs)
            {
                handler.Add(file);
            }
        }

        private IBuilderProvider GetBuilderProvider(IWorkspaceManagerBase workspaceManager)
        {
            BuilderRegistry result = new BuilderRegistry();

            result.Register<ICodeFileLocationProvider>(new WorkspaceCodeFileLocationProviderBuilder(workspaceManager));
            result.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            return result;
        }
    }
}
