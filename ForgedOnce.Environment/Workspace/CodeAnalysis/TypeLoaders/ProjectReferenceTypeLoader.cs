using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace ForgedOnce.Environment.Workspace.CodeAnalysis.TypeLoaders
{
    public class ProjectReferenceTypeLoader : ITypeLoader
    {
        private readonly IWorkspaceManager workspaceManager;
        private readonly IFileSystem fileSystem;
        private readonly AssemblyLoadContext assemblyLoadContext;

        public ProjectReferenceTypeLoader(IWorkspaceManager workspaceManager, IFileSystem fileSystem, AssemblyLoadContext assemblyLoadContext)
        {
            this.workspaceManager = workspaceManager;
            this.fileSystem = fileSystem;
            this.assemblyLoadContext = assemblyLoadContext;
        }

        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            foreach (var reference in this.workspaceManager.GetMetadataReferences<PortableExecutableReference>())
            {
                if (!string.IsNullOrEmpty(reference.FilePath))
                {
                    var referenceFileName = this.fileSystem.Path.GetFileName(reference.FilePath);

                    if (string.Compare(referenceFileName, $"{assemblyName.Name}.dll", true) == 0 ||
                        string.Compare(referenceFileName, $"{assemblyName.Name}.exe", true) == 0)
                    {
                        var referenceAssembly = assemblyLoadContext.LoadFromAssemblyPath(reference.FilePath);
                        
                        return referenceAssembly;
                    }

                }
            }

            return null;
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var assemblyString = typeName.Contains(",") ? typeName.Substring(typeName.IndexOf(",") + 1) : null;
            if (assemblyString != null)
            {
                AssemblyName assemblyName = new AssemblyName(assemblyString);

                var assembly = this.LoadAssembly(assemblyName);

                if (assembly != null)
                {
                    var strippedTypeName = typeName.Substring(0, typeName.IndexOf(","));
                    var result = assembly.GetType(strippedTypeName, false, false);
                    if (result == null)
                    {
                        throw new InvalidOperationException($"Cannot resolve type {strippedTypeName} from {assembly.CodeBase}.");
                    }

                    return result;
                }
            }

            return null;
        }
    }
}
