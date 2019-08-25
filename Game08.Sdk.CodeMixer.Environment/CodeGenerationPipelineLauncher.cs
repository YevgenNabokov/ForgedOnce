using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Builders;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment
{
    public class CodeGenerationPipelineLauncher
    {
        private readonly Workspace workspace;
        private readonly IFileSystem fileSystem;

        public CodeGenerationPipelineLauncher(Workspace workspace, IFileSystem fileSystem)
        {
            this.workspace = workspace;
            this.fileSystem = fileSystem;
        }

        public void Launch(string pipelineConfigurationFilePath)
        {
            if (!fileSystem.File.Exists(pipelineConfigurationFilePath))
            {
                throw new InvalidOperationException($"Pipeline configuration file not found: {pipelineConfigurationFilePath}.");
            }

            var workspaceManager = new WorkspaceManager(this.workspace);

            var processWorkspaceManager = workspaceManager.CreateAdHocClone();
            var basePath = this.fileSystem.Path.GetDirectoryName(pipelineConfigurationFilePath);
            var builderProvider = this.GetBuilderProvider(processWorkspaceManager);

            var pipelineBuilder = new PipelineBuilder(builderProvider, processWorkspaceManager, basePath, this.fileSystem);

            var pipeline = pipelineBuilder.Build(JObject.Parse(this.fileSystem.File.ReadAllText(pipelineConfigurationFilePath)));

            this.ExecutePipeline(pipeline);

            this.SaveOutputs(workspaceManager, pipeline.GetOutputs());
        }

        private void ExecutePipeline(ICodeGenerationPipeline pipeline)
        {
            pipeline.Execute();
        }

        private void SaveOutputs(WorkspaceManager workspaceManager, IEnumerable<CodeFile> outputs)
        {
            WorkspaceFileStorageHandler handler = new WorkspaceFileStorageHandler(workspaceManager);

            foreach (var file in outputs)
            {
                handler.Add(file);
            }
        }

        private IBuilderProvider GetBuilderProvider(IWorkspaceManager workspaceManager)
        {
            BuilderRegistry result = new BuilderRegistry();

            result.Register<ICodeFileLocationProvider>(new WorkspaceCodeFileLocationProviderBuilder(workspaceManager));
            result.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            return result;
        }
    }
}
