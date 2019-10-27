using Game08.Sdk.CodeMixer.CSharp.MsBuild;
using Game08.Sdk.CodeMixer.Glsl.MsBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.MsBuild;
using System;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.WithDefaultAdapters
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystem = new FileSystem();
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, 
                new IMsBuildCodeFileStoreAdapter[]
                {
                    new CSharpMsBuildStoreAdapter(fileSystem),
                    new GlslMsBuildStoreAdapter(fileSystem),
                    new TypeScriptMsBuildStoreAdapter(fileSystem)
                });
            launcher.Execute(args[0], args[1]);
        }
    }
}
