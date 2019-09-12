using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildSolutionStorage : ICodeFileStorageHandler
    {
        private readonly string solutionPath;

        private MsBuildSolution solution;

        public MsBuildSolutionStorage(string solutionPath)
        {
            this.solutionPath = solutionPath;
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
            if (codeFile.Location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = codeFile.Location as WorkspaceCodeFileLocation;
                var targetProj = this.Solution.GetProject(workspaceLocation.ProjectName);

                if (targetProj != null)
                {
                    
                    
                }
            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public void Remove(CodeFile codeFile)
        {
            throw new NotImplementedException();
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            throw new NotImplementedException();
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
