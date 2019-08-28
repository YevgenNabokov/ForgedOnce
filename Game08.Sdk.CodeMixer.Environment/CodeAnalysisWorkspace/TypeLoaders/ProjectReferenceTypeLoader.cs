using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace.TypeLoaders
{
    public class ProjectReferenceTypeLoader : ITypeLoader
    {
        private readonly IWorkspaceManager workspaceManager;
        private readonly IFileSystem fileSystem;

        public ProjectReferenceTypeLoader(IWorkspaceManager workspaceManager, IFileSystem fileSystem)
        {
            this.workspaceManager = workspaceManager;
            this.fileSystem = fileSystem;
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var assemblyString = typeName.Contains(",") ? typeName.Substring(typeName.IndexOf(",") + 1) : null;
            if (assemblyString != null)
            {
                AssemblyName assemblyName = new AssemblyName(assemblyString);

                foreach (var reference in this.workspaceManager.GetMetadataReferences<PortableExecutableReference>())
                {
                    if (!string.IsNullOrEmpty(reference.FilePath))
                    {
                        var referenceFileName = this.fileSystem.Path.GetFileName(reference.FilePath);

                        if (referenceFileName == $"{assemblyName.Name}.dll" || referenceFileName == $"{assemblyName.Name}.exe")
                        {
                            var referenceAssembly = Assembly.LoadFrom(reference.FilePath);
                            var result = referenceAssembly.GetType(typeName, false, false);
                            if (result == null)
                            {
                                throw new InvalidOperationException($"Cannot resolve type {typeName} from {reference.FilePath}.");
                            }

                            return result;
                        }

                    }
                }
            }            

            return null;
        }
    }
}
