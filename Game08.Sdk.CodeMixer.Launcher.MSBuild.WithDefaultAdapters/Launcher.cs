using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.CSharp.MsBuild;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Glsl.MsBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.MsBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.WithDefaultAdapters
{
    public class Launcher
    {
        public Launcher(FileSystem fileSystem = null, ILogger logger = null)
        {
            this.FileSystem = fileSystem ?? new FileSystem();
            this.Logger = logger ?? new CollectionLogger();
        }

        public FileSystem FileSystem { get; protected set; }

        public ILogger Logger { get; protected set; }

        public void Launch(string solutionPath, string pipelineConfigurationPath)
        {
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(
                    this.FileSystem,
                    this.Logger,
                    new IMsBuildCodeFileStoreAdapter[]
                    {
                    new CSharpMsBuildStoreAdapter(this.FileSystem),
                    new GlslMsBuildStoreAdapter(this.FileSystem),
                    new TypeScriptMsBuildStoreAdapter(this.FileSystem)
                    });
            launcher.Execute(solutionPath, pipelineConfigurationPath);
        }
    }
}
