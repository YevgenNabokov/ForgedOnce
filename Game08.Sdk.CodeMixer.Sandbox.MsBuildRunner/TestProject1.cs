﻿using Game08.Sdk.CodeMixer.CSharp.MsBuild;
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
            var adapters = new IMsBuildCodeFileStoreAdapter[] { new TypeScriptMsBuildStoreAdapter(fileSystem), new CSharpMsBuildStoreAdapter(fileSystem) };
            CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, adapters);
            var testSolutionPath = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SimplePluginTest.sln";
            var testConfig = "..\\..\\..\\..\\SandboxProjects\\TestProjects.SimplePluginTest\\TestProjects.SubjectProj\\CGPipeline.json";
            launcher.Execute(testSolutionPath, testConfig);
        }
    }
}
