using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Microsoft.CodeAnalysis;
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
            throw new NotImplementedException();
        }
    }
}
