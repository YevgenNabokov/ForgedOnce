using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ITypeLoader
    {
        Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null);

        Assembly LoadAssembly(AssemblyName assemblyName);
    }
}
