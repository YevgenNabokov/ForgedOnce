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

        public Dictionary<Guid, Compilation> CompileProjects(IEnumerable<Guid> projectIds)
        {
            Dictionary<Guid, Compilation> result = new Dictionary<Guid, Compilation>();

            var chains = this.workspaceManager.GetProjectsDependencyChains(projectIds);

            foreach (var chain in chains)
            {
                foreach (var id in chain)
                {
                    if (!result.ContainsKey(id))
                    {
                        var project = this.workspaceManager.FindProject(id);
                        var compilation = project.GetCompilationAsync().Result;
                        result.Add(id, compilation);
                    }
                }
            }

            return result;
        }
    }
}
