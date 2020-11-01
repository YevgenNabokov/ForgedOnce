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
    public class TestProject1 : ISandboxProject
    {
        public string Name => "Simple C# to C# code generation project";

        public void Run()
        {
            var fileSystem = new FileSystem();
            var logger = new CollectionLogger();
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new GlslMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, logger, adapters);

            //var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SimplePluginTest.sln";
            //var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SubjectProj\\CGPipeline.json";

            var testSolutionPath = @"f:\Projects\ForgedOnce\ForgedOnce.TSLanguageServices\ForgedOnce.TsLanguageServices.FullAst.sln";
            var testConfig = @"f:\Projects\ForgedOnce\ForgedOnce.TSLanguageServices\ForgedOnce.TsLanguageServices.CodeGeneration\CGPipeline_FullAst.json";

            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
