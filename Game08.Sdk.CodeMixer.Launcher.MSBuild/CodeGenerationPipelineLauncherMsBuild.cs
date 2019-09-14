﻿using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild
{
    public class CodeGenerationPipelineLauncherMsBuild
    {
        private readonly IFileSystem fileSystem;

        private readonly IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters;

        public CodeGenerationPipelineLauncherMsBuild(IFileSystem fileSystem, IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters = null)
        {
            this.fileSystem = fileSystem;
            this.msBuildStoreAdapters = msBuildStoreAdapters ?? new List<IMsBuildCodeFileStoreAdapter>() { new DefaultItemStoreAdapter(fileSystem) };
        }

        public void Execute(string solutionPath, string pipelineConfigurationPath)
        {
            var vsi = MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var outputStorage = new MsBuildSolutionStorage(this.fileSystem.Path.GetFullPath(solutionPath), this.msBuildStoreAdapters);

            var launcher = new CodeGenerationPipelineLauncher(workspace, this.fileSystem, null, outputStorage);
            launcher.Launch(pipelineConfigurationPath);

            outputStorage.Save();
        }
    }
}
