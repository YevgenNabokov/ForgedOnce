using ForgedOnce.CSharp.MsBuild;
using ForgedOnce.Launcher.MSBuild;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using ForgedOnce.TypeScript.MsBuild;
using ForgedOnce.Environment;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Sandbox.MsBuildRunner
{
    public class TestProject4_Metadata
    {
        public static void Run()
        {
            var fileSystem = new FileSystem();
            var logger = new CollectionLogger();
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, logger, adapters);
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.MetadataTest\\MetadataTest.All.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.MetadataTest\\Source\\CGPipeline.json";
            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
