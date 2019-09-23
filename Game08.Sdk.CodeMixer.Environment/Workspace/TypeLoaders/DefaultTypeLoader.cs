using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace.TypeLoaders
{
    public class DefaultTypeLoader : ITypeLoader
    {
        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            var result = Type.GetType(typeName, false, false);
            return result;
        }
    }
}
