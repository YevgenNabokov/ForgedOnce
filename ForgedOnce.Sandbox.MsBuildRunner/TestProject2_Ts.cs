using ForgedOnce.CSharp.MsBuild;
using ForgedOnce.Environment;
using ForgedOnce.Launcher.MSBuild;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using ForgedOnce.TypeScript.MsBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Sandbox.MsBuildRunner
{
    public class TestProject2_Ts : ISandboxProject
    {
        public string Name => "Simple C# to TypeScript code generation project";

        public void Run()
        {
            var fileSystem = new FileSystem();
            var logger = new CollectionLogger();
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, logger, adapters);
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.LTSPluginTest\\LTSPluginTest.All.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.LTSPluginTest\\Subject\\CGPipeline.json";
        
            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
