using ForgedOnce.CSharp.MsBuild;
using ForgedOnce.Environment;
using ForgedOnce.Glsl.MsBuild;
using ForgedOnce.Launcher.MSBuild;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using ForgedOnce.TypeScript.MsBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Sandbox.MsBuildRunner
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
