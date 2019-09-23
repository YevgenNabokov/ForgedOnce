using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders
{
    public class WorkspaceTypeLoader : ITypeLoader
    {
        private readonly IWorkspaceManager workspaceManager;

        private readonly WorkspaceCompilationHandler compilationHandler;

        public WorkspaceTypeLoader(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
            this.compilationHandler = new WorkspaceCompilationHandler(workspaceManager);
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var assemblyString = typeName.Contains(",") ? typeName.Substring(typeName.IndexOf(",") + 1) : null;
            if (assemblyString != null)
            {
                AssemblyName assemblyName = new AssemblyName(assemblyString);

                var project = this.workspaceManager.FindProjectByAssemblyName(assemblyName.Name);

                if (project != null)
                {
                    var compilations = this.compilationHandler.CompileProjects(new[] { project.Name });
                    using (var stream = new MemoryStream())
                    {
                        compilations[project.Name].Emit(stream);
                        Assembly assembly = Assembly.Load(stream.GetBuffer());

                        var strippedTypeName = typeName.Substring(0, typeName.IndexOf(","));
                        var result = assembly.GetType(strippedTypeName, false, false);
                        if (result == null)
                        {
                            throw new InvalidOperationException($"Cannot resolve type {strippedTypeName} from compiled solution project {project.Name}.");
                        }

                        return result;
                    }
                }
            }

            return null;
        }
    }
}
