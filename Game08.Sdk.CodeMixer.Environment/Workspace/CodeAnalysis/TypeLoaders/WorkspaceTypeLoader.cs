using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders
{
    public class WorkspaceTypeLoader : ITypeLoader
    {
        private readonly IWorkspaceManager workspaceManager;
        private readonly AssemblyLoadContext assemblyLoadContext;
        private readonly bool debuggingEnabled;
        private readonly WorkspaceCompilationHandler compilationHandler;

        private readonly Dictionary<AssemblyName, Assembly> loadedAssemblies = new Dictionary<AssemblyName, Assembly>();

        public WorkspaceTypeLoader(IWorkspaceManager workspaceManager, AssemblyLoadContext assemblyLoadContext, bool debuggingEnabled = true)
        {
            this.workspaceManager = workspaceManager;
            this.assemblyLoadContext = assemblyLoadContext;
            this.debuggingEnabled = debuggingEnabled;
            this.compilationHandler = new WorkspaceCompilationHandler(workspaceManager);
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            AssemblyName assemblyName = this.GetAssemblyNameFromTypeName(typeName);
            var assembly = this.LoadAssembly(assemblyName);

            if (assembly != null)
            {
                var strippedTypeName = typeName.Substring(0, typeName.IndexOf(","));

                var loadedType = assembly.GetType(strippedTypeName, false, false);
                if (loadedType == null)
                {
                    throw new InvalidOperationException($"Cannot resolve type {strippedTypeName} from compiled assembly {assemblyName}.");
                }

                return loadedType;
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

                    var emitResult = compilation.Emit(stream, symbolsStream, options: emitOptions);
                    this.AssertEmitResult(emitResult);

                    stream.Seek(0, SeekOrigin.Begin);
                    symbolsStream?.Seek(0, SeekOrigin.Begin);
                    var assembly = this.assemblyLoadContext.LoadFromStream(stream, symbolsStream);
                    return assembly;
                }
            }
        }

        private void AlertBuildErrors(Diagnostic[] errors)
        {
            var diagnostics = string.Join(",", errors.Select(d => d.ToString()));
            throw new InvalidOperationException($"Error occurred on build: {diagnostics}");
        }

        private void AssertEmitResult(EmitResult result)
        {
            if (!result.Success)
            {
                var diagnostics = string.Join(",", result.Diagnostics.Select(d => d.ToString()));
                throw new InvalidOperationException($"Error occurred on assembly Emit: {diagnostics}");
            }
        }

        private Assembly EmitAndLoad(Compilation compilation)
        {
            using (var stream = new MemoryStream())
            {
                compilation.Emit(stream);
                Assembly assembly = this.assemblyLoadContext.LoadFromStream(stream);
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

        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            foreach (var name in this.loadedAssemblies.Keys)
            {
                if (AssemblyName.ReferenceMatchesDefinition(assemblyName, name))
                {
                    return this.loadedAssemblies[name];
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
                        var errors = compilations[project.Name].GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
                        if (errors.Length > 0)
                        {
                            this.AlertBuildErrors(errors);
                        }

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

                        return assembly;
                    }
                }
            }

            return null;
        }
    }
}
