using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace ForgedOnce.Environment.Workspace.TypeLoaders
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly ITypeLoader typeLoader;

        public CustomAssemblyLoadContext(ITypeLoader typeLoader)
        {
            this.typeLoader = typeLoader;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return this.typeLoader.LoadAssembly(assemblyName);
        }
    }
}
