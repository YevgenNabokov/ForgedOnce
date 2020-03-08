using ForgedOnce.Launcher.MSBuild;
using System;
using System.IO.Abstractions;

namespace ForgedOnce.Sandbox.MsBuildRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            TestProject1.Run();
            ////TestProject2_Lts.Run();
            ////TestProject3_Glsl.Run();
            ////TestProject4_Metadata.Run();
        }
    }
}
