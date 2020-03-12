using ForgedOnce.Core.Interfaces;
using ForgedOnce.CSharp.MsBuild;
using ForgedOnce.Environment;
using ForgedOnce.Glsl.MsBuild;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using ForgedOnce.TypeScript.MsBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild.Default
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
