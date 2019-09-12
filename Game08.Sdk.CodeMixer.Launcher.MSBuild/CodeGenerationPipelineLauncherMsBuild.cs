using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild
{
    public class CodeGenerationPipelineLauncherMsBuild
    {
        private readonly IFileSystem fileSystem;

        public CodeGenerationPipelineLauncherMsBuild(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Execute(string solutionPath, string pipelineConfigurationPath)
        {
            var vsi = MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var outputStorage = new MsBuildSolutionStorage(this.fileSystem.Path.GetFullPath(solutionPath));

            var launcher = new CodeGenerationPipelineLauncher(workspace, this.fileSystem, null, outputStorage);
            launcher.Launch(pipelineConfigurationPath);
        }
    }
}
