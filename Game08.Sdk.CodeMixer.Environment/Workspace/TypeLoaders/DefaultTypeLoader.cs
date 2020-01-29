using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.TypeLoaders
{
    public class DefaultTypeLoader : ITypeLoader
    {
        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(AssemblyName.ReferenceMatchesDefinition(assemblyName, assembly.GetName()))
                {
                    return assembly;
                }
            }

            return null;
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var result = Type.GetType(typeName, false, false);
            return result;
        }
    }
}
