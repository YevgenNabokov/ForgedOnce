using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis
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
            Dictionary<string, List<MetadataReference>> projectReferences = new Dictionary<string, List<MetadataReference>>();

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

                        projectReferences.Add(name, new List<MetadataReference>(project.MetadataReferences));

                        if (chainReferences.Count > 0)
                        {
                            project = project.WithMetadataReferences(chainReferences);
                        }

                        var compilation = project.GetCompilationAsync().Result;

                        compilationReferences.Add(name, compilation.ToMetadataReference());

                        result.Add(name, compilation);
                    }

                    chainReferences.AddRange(projectReferences[name]);
                    chainReferences.Add(compilationReferences[name]);
                }
            }

            return result;
        }
    }
}
