using System;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(new FileSystem());
            launcher.Execute(args[0], args[1]);
        }
    }
}
