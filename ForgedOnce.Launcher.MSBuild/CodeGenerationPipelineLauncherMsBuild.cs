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
using System.Linq;
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

            this.AssertWorkspaceErrors(workspace);

            var solutionManager = new MsBuildSolutionStorage(this.fileSystem.Path.GetFullPath(solutionPath), this.msBuildStoreAdapters, this.fileSystem);

            var launcher = new CodeGenerationPipelineLauncher(new WorkspaceManager(workspace), solutionManager, this.fileSystem, null, solutionManager, this.logger);
            launcher.Launch(pipelineConfigurationPath);

            solutionManager.Save();
        }

        private void AssertWorkspaceErrors(MSBuildWorkspace workspace)
        {
            var failures = workspace.Diagnostics.Where(d => d.Kind == Microsoft.CodeAnalysis.WorkspaceDiagnosticKind.Failure).ToList();
            if (failures.Count > 0)
            {
                var message = string.Join("\r\n", failures.Select(f => f.Message));
                throw new InvalidOperationException($"Opened solution contains errors:\r\n{message}");
            }
        }
    }
}
