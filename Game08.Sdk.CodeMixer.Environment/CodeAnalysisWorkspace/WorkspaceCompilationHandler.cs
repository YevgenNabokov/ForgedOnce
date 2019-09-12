using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceCompilationHandler
    {
        protected readonly IWorkspaceManager workspaceManager;

        public WorkspaceCompilationHandler(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public Dictionary<string, Compilation> CompileProjects(IEnumerable<string> projectNames)
        {
            Dictionary<string, Compilation> result = new Dictionary<string, Compilation>();

            var chains = this.workspaceManager.GetProjectsDependencyChains(projectNames);

            foreach (var chain in chains)
            {
                foreach (var name in chain)
                {
                    if (!result.ContainsKey(name))
                    {
                        var project = this.workspaceManager.FindProject(name);
                        var compilation = project.GetCompilationAsync().Result;
                        result.Add(name, compilation);
                    }
                }
            }

            return result;
        }
    }
}
