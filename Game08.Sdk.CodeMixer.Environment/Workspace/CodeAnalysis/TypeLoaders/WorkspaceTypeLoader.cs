using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders
{
    public class WorkspaceTypeLoader : ITypeLoader
    {
        private readonly IWorkspaceManager workspaceManager;
        private readonly bool debuggingEnabled;
        private readonly WorkspaceCompilationHandler compilationHandler;

        private readonly Dictionary<AssemblyName, Assembly> loadedAssemblies = new Dictionary<AssemblyName, Assembly>();

        public WorkspaceTypeLoader(IWorkspaceManager workspaceManager, bool debuggingEnabled = true)
        {
            this.workspaceManager = workspaceManager;
            this.debuggingEnabled = debuggingEnabled;
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
                    using (var symbolsStream = new MemoryStream())
                    {
                        using (var stream = new MemoryStream())
                        {
                            Assembly assembly = null;
                            if (debuggingEnabled)
                            {
                                var pdbFileName = $"{assemblyName.Name}.pdb";
                                assembly = this.EmitAndLoadWithPdb(compilations[project.Name], pdbFileName);
                            }
                            else
                            {
                                assembly = this.EmitAndLoad(compilations[project.Name]);
                            }

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
            }

            return null;
        }

        private Assembly EmitAndLoadWithPdb(Compilation compilation, string pdbFileName)
        {
            using (var symbolsStream = new MemoryStream())
            {
                using (var stream = new MemoryStream())
                {
                    var emitOptions = new EmitOptions(
                        debugInformationFormat: DebugInformationFormat.PortablePdb,
                        pdbFilePath: pdbFileName);

                    compilation.Emit(stream, symbolsStream, options: emitOptions);
                    stream.Seek(0, SeekOrigin.Begin);
                    symbolsStream?.Seek(0, SeekOrigin.Begin);
                    var assembly = AssemblyLoadContext.Default.LoadFromStream(stream, symbolsStream);
                    return assembly;
                }
            }
        }
        private Assembly EmitAndLoad(Compilation compilation)
        {
            using (var stream = new MemoryStream())
            {
                compilation.Emit(stream);
                Assembly assembly = Assembly.Load(stream.GetBuffer());
                return assembly;
            }
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
