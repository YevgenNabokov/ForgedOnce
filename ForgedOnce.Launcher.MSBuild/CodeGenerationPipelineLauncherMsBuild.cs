using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Workspace.CodeAnalysis;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using ForgedOnce.Launcher.MSBuild.Storage;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild
{
    public class CodeGenerationPipelineLauncherMsBuild
    {
        private readonly IFileSystem fileSystem;
        private readonly ILogger logger;
        private readonly IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters;

        public CodeGenerationPipelineLauncherMsBuild(IFileSystem fileSystem, ILogger logger, IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters = null)
        {
            this.fileSystem = fileSystem;
            this.logger = logger;
            this.msBuildStoreAdapters = msBuildStoreAdapters ?? new List<IMsBuildCodeFileStoreAdapter>() { new DefaultItemStoreAdapter(fileSystem) };
        }

        public void Execute(string solutionPath, string pipelineConfigurationPath)
        {
            var vsi = MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();            
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var solutionManager = new MsBuildSolutionStorage(this.fileSystem.Path.GetFullPath(solutionPath), this.msBuildStoreAdapters, this.fileSystem);

            var launcher = new CodeGenerationPipelineLauncher(new WorkspaceManager(workspace), solutionManager, this.fileSystem, null, solutionManager, this.logger);
            launcher.Launch(pipelineConfigurationPath);

            solutionManager.Save();
        }
    }
}
