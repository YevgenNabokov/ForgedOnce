using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public static class PathHelperVisitorMap
    {
        private static object buildLockObject = new object();

        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> map;

        public static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> Map
        {
            get
            {
                lock (buildLockObject)
                {
                    if (map == null)
                    {
                        map = BuildMap();
                    }
                }

                return map;
            }
        }

        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> BuildMap()
        {
            var cSharpSyntaxNamespace = $"{nameof(Microsoft)}.{nameof(Microsoft.CodeAnalysis)}.{nameof(Microsoft.CodeAnalysis.CSharp)}.{nameof(Microsoft.CodeAnalysis.CSharp.Syntax)}";
            Dictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> result = new Dictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>>();

            foreach (var type in typeof(ClassDeclarationSyntax).Assembly.GetTypes())
            {
                if (type.Namespace == cSharpSyntaxNamespace)
                {
                    Dictionary<string, PathHelperVisitorMapBranchInfo> typeInfo = new Dictionary<string, PathHelperVisitorMapBranchInfo>();
                    foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        Type itemType = p.PropertyType;
                        bool isCollection = false;

                        var collectionType = p.PropertyType.GetInterfaces().FirstOrDefault(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyList<>));
                        if (collectionType != null)
                        {
                            isCollection = true;
                            itemType = collectionType.GetGenericArguments().First();
                        }

                        if (typeof(SyntaxNode).IsAssignableFrom(itemType))
                        {
                            typeInfo.Add(p.Name, new PathHelperVisitorMapBranchInfo(p, itemType, false, isCollection));
                            continue;
                        }

                        if (typeof(SyntaxToken).IsAssignableFrom(itemType))
                        {
                            typeInfo.Add(p.Name, new PathHelperVisitorMapBranchInfo(p, itemType, true, isCollection));
                            continue;
                        }
                    }

                    if (typeInfo.Count > 0)
                    {
                        result.Add(type, typeInfo);
                    }
                }
            }

            return result;
        }
    }
}
