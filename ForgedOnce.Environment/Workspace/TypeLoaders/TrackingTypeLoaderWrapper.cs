using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace ForgedOnce.Environment.Workspace.TypeLoaders
{
    public class TrackingTypeLoaderWrapper : ITypeLoader
    {
        private readonly ITypeLoader underlyingTypeLoader;
        private readonly IFileSystem fileSystem;
        private readonly AssemblyLoadContext assemblyLoadContext;
        private HashSet<string> probingPaths = new HashSet<string>();

        private ResolveEventHandler resolveEventHandler;

        public TrackingTypeLoaderWrapper(ITypeLoader underlyingTypeLoader, IFileSystem fileSystem, AssemblyLoadContext assemblyLoadContext)
        {
            this.underlyingTypeLoader = underlyingTypeLoader;
            this.fileSystem = fileSystem;
            this.assemblyLoadContext = assemblyLoadContext;
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var result = this.underlyingTypeLoader.LoadType(typeName, nugetPackageName, nugetPackageVersion);
            if (result != null)
            {
                var location = result.Assembly.Location;
                if (!string.IsNullOrEmpty(location))
                {
                    var fullPath = this.fileSystem.Path.GetFullPath(location);
                    var folder = this.fileSystem.Path.GetDirectoryName(fullPath);
                    if (!this.probingPaths.Contains(folder))
                    {
                        this.probingPaths.Add(folder);
                    }
                }
            }

            return result;
        }

        private Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return this.LoadAssembly(new AssemblyName(args.Name));
        }

        public void AttachAssemblyResolveHandler()
        {
            if (resolveEventHandler == null)
            {
                this.resolveEventHandler = new ResolveEventHandler(HandleAssemblyResolve);
                AppDomain.CurrentDomain.AssemblyResolve += this.resolveEventHandler;
            }
        }

        public void DetachAssemblyResolveHandler()
        {
            if (this.resolveEventHandler != null)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= this.resolveEventHandler;
                this.resolveEventHandler = null;
            }
        }

        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName.FullName);
            if (assembly != null)
                return assembly;

            string fileName = assemblyName.FullName.Split(',')[0] + ".dll".ToLower();

            foreach (var probingPath in this.probingPaths)
            {
                var path = this.fileSystem.Path.Combine(probingPath, fileName);
                try
                {
                    if (this.fileSystem.File.Exists(path))
                    {
                        var result = this.assemblyLoadContext.LoadFromAssemblyPath(path);
                        return result;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
