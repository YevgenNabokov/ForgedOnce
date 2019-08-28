﻿using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace.TypeLoaders
{
    public class AggregateTypeLoader : ITypeLoader
    {
        private List<ITypeLoader> typeResolvers;

        public AggregateTypeLoader(params ITypeLoader[] typeResolvers)
        {
            this.typeResolvers = new List<ITypeLoader>(typeResolvers);
        }

        public void AddResolver(ITypeLoader typeResolver)
        {
            this.typeResolvers.Add(typeResolver);
        }

        public Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null)
        {
            foreach (var resolver in this.typeResolvers)
            {
                var result = resolver.LoadType(typeName, nugetPackageName, nugetPackageVersion);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
