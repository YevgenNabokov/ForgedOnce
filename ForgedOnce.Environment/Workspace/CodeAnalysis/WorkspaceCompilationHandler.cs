using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeAnalysis
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
            Dictionary<string, CompilationReference> compilationReferences = new Dictionary<string, CompilationReference>();

            var chains = this.workspaceManager.GetProjectsDependencyChains(projectNames);

            List<MetadataReference> chainReferences = new List<MetadataReference>();

            foreach (var chain in chains)
            {
                chainReferences.Clear();
                foreach (var name in chain)
                {
                    if (!result.ContainsKey(name))
                    {
                        var project = this.workspaceManager.FindProject(name);

                        if (chainReferences.Count > 0)
                        {
                            project = project.WithMetadataReferences(project.MetadataReferences.Concat(chainReferences));
                        }

                        var compilation = project.GetCompilationAsync().Result;

                        compilationReferences.Add(name, compilation.ToMetadataReference());

                        result.Add(name, compilation);
                    }

                    chainReferences.Add(compilationReferences[name]);
                }
            }

            return result;
        }
    }
}
