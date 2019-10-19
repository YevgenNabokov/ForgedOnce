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

        private readonly Dictionary<AssemblyName, Assembly> loadedAssemblies = new Dictionary<AssemblyName, Assembly>();

        public WorkspaceTypeLoader(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
            this.compilationHandler = new WorkspaceCompilationHandler(workspaceManager);
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            AssemblyName assemblyName = this.GetAssemblyNameFromTypeName(typeName);
            if (assemblyName != null)
            {
                var strippedTypeName = typeName.Substring(0, typeName.IndexOf(","));

                foreach (var name in this.loadedAssemblies.Keys)
                {
                    if (name.Name == assemblyName.Name)
                    {
                        var loadedType = this.loadedAssemblies[name].GetType(strippedTypeName, false, false);
                        if (loadedType == null)
                        {
                            throw new InvalidOperationException($"Cannot resolve type {strippedTypeName} from compiled assembly {name}.");
                        }

                        return loadedType;
                    }
                }

                var project = this.workspaceManager.FindProjectByAssemblyName(assemblyName.Name);

                if (project != null)
                {
                    var compilations = this.compilationHandler.CompileProjects(new[] { project.Name });
                    using (var stream = new MemoryStream())
                    {
                        compilations[project.Name].Emit(stream);
                        Assembly assembly = Assembly.Load(stream.GetBuffer());

                        this.loadedAssemblies.Add(assembly.GetName(), assembly);
                        
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

        private AssemblyName GetAssemblyNameFromTypeName(string typeName)
        {
            var assemblyString = typeName.Contains(",") ? typeName.Substring(typeName.IndexOf(",") + 1) : null;
            if (assemblyString != null)
            {
                return new AssemblyName(assemblyString);
            }

            return null;
        }
    }
}
