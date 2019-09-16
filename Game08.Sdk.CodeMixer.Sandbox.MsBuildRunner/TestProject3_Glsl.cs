using Game08.Sdk.CodeMixer.CSharp.MsBuild;
using Game08.Sdk.CodeMixer.Glsl.MsBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Sandbox.MsBuildRunner
{
    public class TestProject3_Glsl
    {
        public static void Run()
        {
            var fileSystem = new FileSystem();
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new GlslMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, adapters);
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.GlslPluginTest\\GlslPluginTest.All.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.GlslPluginTest\\Subject\\CGPipeline.json";

            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
