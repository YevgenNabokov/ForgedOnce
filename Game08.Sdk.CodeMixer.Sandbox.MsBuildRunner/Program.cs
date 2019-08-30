using Game08.Sdk.CodeMixer.Launcher.MSBuild;
using System;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Sandbox.MsBuildRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(new FileSystem());
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SimplePluginTest.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SubjectProj\\CGPipeline.json";
            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
