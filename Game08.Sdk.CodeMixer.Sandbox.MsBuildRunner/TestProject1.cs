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
    public class TestProject1
    {
        public static void Run()
        {
            var fileSystem = new FileSystem();
            var logger = new CollectionLogger();
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new GlslMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, logger, adapters);
            ////var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SimplePluginTest.sln";
            ////var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SubjectProj\\CGPipeline.json";


            var testSolutionPath = @"g:\Projects\Game08\Src\Sdk\Game08.Sdk.CSToTS\Game08.Sdk.CSToTS.SyntaxTrees.sln";
            var testConfig = @"g:\Projects\Game08\Src\Sdk\Game08.Sdk.CSToTS\Game08.Sdk.CodeGeneration\CGPipeline.json";

            ////var testSolutionPath = @"g:\Projects\Game08\Src\Framework\Sandbox\MvcApp\TestApp.All.sln";
            ////var testConfig = @"g:\Projects\Game08\Src\Framework\Sandbox\MvcApp\TestApp.CodeGeneration\CGPipeline_pre.json";

            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
