using Game08.Sdk.CodeMixer.Launcher.MSBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Sandbox.MsBuildRunner
{
    public class TestProject2_Lts
    {
        public static void Run()
        {
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(new FileSystem());
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.LTSPluginTest\\LTSPluginTest.All.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.LTSPluginTest\\Subject\\CGPipeline.json";
        
            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
