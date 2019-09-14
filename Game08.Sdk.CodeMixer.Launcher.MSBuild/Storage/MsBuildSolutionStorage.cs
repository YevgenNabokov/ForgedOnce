using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildSolutionStorage : ICodeFileStorageHandler
    {
        private readonly string solutionPath;
        private readonly IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters;
        private MsBuildSolution solution;

        public MsBuildSolutionStorage(string solutionPath, IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters)
        {
            this.solutionPath = solutionPath;
            this.msBuildStoreAdapters = msBuildStoreAdapters;
        }

        private MsBuildSolution Solution
        {
            get
            {
                if (this.solution == null)
                {
                    this.solution = MsBuildSolution.Load(this.solutionPath);
                }

                return this.solution;
            }
        }

        public void Add(CodeFile codeFile)
        {
            var targetProj = this.GetContainingProject(codeFile.Location);

            if (targetProj != null)
            {
                foreach (var adapter in this.msBuildStoreAdapters)
                {
                    if (adapter.CodeFileSupported(codeFile))
                    {
                        adapter.AddOrUpdate(codeFile, targetProj);
                        return;
                    }
                }

                throw new InvalidOperationException($"No MsBuild store adapter was found for {codeFile}.");
            }
        }

        public void Remove(CodeFile codeFile)
        {
            var targetProj = this.GetContainingProject(codeFile.Location);
            if (targetProj != null)
            {
                foreach (var adapter in this.msBuildStoreAdapters)
                {
                    if (adapter.CodeFileSupported(codeFile))
                    {
                        adapter.Remove(codeFile, targetProj);
                        return;
                    }
                }
            }
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            throw new NotImplementedException();
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            foreach(var proj in this.solution.Projects)
            {
                proj.Value.Project.Save();
            }
        }

        private MsBuildProject GetContainingProject(CodeFileLocation location)
        {
            if (location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = location as WorkspaceCodeFileLocation;
                return this.Solution.GetProject(workspaceLocation.ProjectName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
