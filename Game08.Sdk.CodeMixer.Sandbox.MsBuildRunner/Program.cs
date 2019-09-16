using Game08.Sdk.CodeMixer.Launcher.MSBuild;
using System;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Sandbox.MsBuildRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            ////TestProject1.Run();
            ////TestProject2_Lts.Run();
            TestProject3_Glsl.Run();
        }
    }
}
