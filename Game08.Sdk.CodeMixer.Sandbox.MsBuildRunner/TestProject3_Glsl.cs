using Game08.Sdk.CodeMixer.CSharp.MsBuild;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Glsl.MsBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.MsBuild;
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
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new GlslMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            var logger = new CollectionLogger();
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, logger, adapters);
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.GlslPluginTest\\GlslPluginTest.All.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.GlslPluginTest\\Subject\\CGPipeline.json";

            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
