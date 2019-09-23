using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders
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

                        if (string.Compare(referenceFileName, $"{assemblyName.Name}.dll", true) == 0 ||
                            string.Compare(referenceFileName, $"{assemblyName.Name}.exe", true) == 0)
                        {
                            var strippedTypeName = typeName.Substring(0, typeName.IndexOf(","));
                            var referenceAssembly = Assembly.Load(this.fileSystem.File.ReadAllBytes(reference.FilePath));
                            var result = referenceAssembly.GetType(strippedTypeName, false, false);
                            if (result == null)
                            {
                                throw new InvalidOperationException($"Cannot resolve type {strippedTypeName} from {reference.FilePath}.");
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
