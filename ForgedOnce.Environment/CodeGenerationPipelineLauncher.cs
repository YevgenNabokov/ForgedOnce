﻿using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Builders;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Workspace.CodeAnalysis;
using ForgedOnce.Environment.Workspace.CodeAnalysis.TypeLoaders;
using ForgedOnce.Environment.Workspace.TypeLoaders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace ForgedOnce.Environment
{
    public class CodeGenerationPipelineLauncher
    {
        private readonly IWorkspaceManager initialWorkspaceManager;
        private readonly IWorkspaceManagerBase outputWorkspaceManager;
        private readonly IFileSystem fileSystem;
        private readonly ITypeLoader additionalTypeLoader;
        private readonly ICodeFileStorageHandler outputStorageHandler;
        private readonly ILogger logger;

        public CodeGenerationPipelineLauncher(
            IWorkspaceManager initialWorkspaceManager,
            IWorkspaceManagerBase outputWorkspaceManager,
            IFileSystem fileSystem,
            ITypeLoader additionalTypeLoader = null,
            ICodeFileStorageHandler outputStorageHandler = null,
            ILogger logger = null)
        {
            this.initialWorkspaceManager = initialWorkspaceManager;
            this.outputWorkspaceManager = outputWorkspaceManager;
            this.fileSystem = fileSystem;
            this.additionalTypeLoader = additionalTypeLoader;
            this.outputStorageHandler = outputStorageHandler ?? outputWorkspaceManager;
            this.logger = logger ?? new TextLogger(this.fileSystem);
        }

        public void Launch(string pipelineConfigurationFilePath)
        {
            if (!fileSystem.File.Exists(pipelineConfigurationFilePath))
            {
                throw new InvalidOperationException($"Pipeline configuration file not found: {pipelineConfigurationFilePath}.");
            }

            PipelineWorkspaceManagersWrapper workspaces = new PipelineWorkspaceManagersWrapper(
                this.initialWorkspaceManager.CreateAdHocClone(),
                this.outputWorkspaceManager);

            var typeLoader = new AggregateTypeLoader();
            var assemblyLoadContext = new CustomAssemblyLoadContext(typeLoader);

            typeLoader.AddResolver(new DefaultTypeLoader());
            typeLoader.AddResolver(new ProjectReferenceTypeLoader(workspaces.ProcessingWorkspace, this.fileSystem, assemblyLoadContext));
            typeLoader.AddResolver(new WorkspaceTypeLoader(workspaces.ProcessingWorkspace, assemblyLoadContext));
            if (this.additionalTypeLoader != null)
            {
                typeLoader.AddResolver(this.additionalTypeLoader);
            }

            var trackingTypeLoader = new TrackingTypeLoaderWrapper(typeLoader, this.fileSystem, assemblyLoadContext);

            trackingTypeLoader.AttachAssemblyResolveHandler();
            
            var basePath = this.fileSystem.Path.GetDirectoryName(this.fileSystem.Path.GetFullPath(pipelineConfigurationFilePath));
            var builderProvider = this.GetBuilderProvider(workspaces, basePath);

            var pipelineBuilder = new PipelineBuilder(
                builderProvider,
                workspaces,
                basePath,
                this.fileSystem,
                trackingTypeLoader,
                this.logger);

            var pipeline = pipelineBuilder.Build(JObject.Parse(this.fileSystem.File.ReadAllText(pipelineConfigurationFilePath)));

            this.ExecutePipeline(pipeline);

            this.SaveOutputs(this.outputStorageHandler, pipeline.GetOutputs());
        }

        private void ExecutePipeline(ICodeGenerationPipeline pipeline)
        {
            pipeline.Execute();
        }

        private void SaveOutputs(ICodeFileStorageHandler handler, IEnumerable<CodeFile> outputs)
        {
            foreach (var file in outputs)
            {
                handler.Add(file);
            }
        }

        private IBuilderProvider GetBuilderProvider(IPipelineWorkspaceManagers workspaces, string basePath)
        {
            BuilderRegistry result = new BuilderRegistry();

            result.Register<ICodeFileDestination>(new WorkspaceCodeFileDestinationBuilder(workspaces));
            result.Register<ICodeFileDestination>(new FileSystemCodeFileDestinationBuilder(this.fileSystem, basePath), "fileSystem");
            result.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            return result;
        }
    }
}
