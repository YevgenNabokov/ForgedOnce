using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public static class SyntaxTreeMapper
    {
        public static readonly HashSet<string> ExcludedPropertyNames = new HashSet<string>() { "Parent" };

        private static object buildLockObject = new object();

        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> map;

        public static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> Map
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

        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> BuildMap()
        {
            var cSharpSyntaxNamespace = string.Join(".", nameof(Microsoft), nameof(Microsoft.CodeAnalysis), nameof(Microsoft.CodeAnalysis.CSharp), nameof(Microsoft.CodeAnalysis.CSharp.Syntax));
            Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> result = new Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>>();

            foreach (var type in typeof(ClassDeclarationSyntax).Assembly.GetTypes())
            {
                if (type.Namespace == cSharpSyntaxNamespace)
                {
                    Dictionary<string, SyntaxTreeMapBranchInfo> typeInfo = new Dictionary<string, SyntaxTreeMapBranchInfo>();
                    foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (!ExcludedPropertyNames.Contains(p.Name))
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
                                typeInfo.Add(p.Name, new SyntaxTreeMapBranchInfo(p, itemType, false, isCollection));
                                continue;
                            }

                            if (typeof(SyntaxToken).IsAssignableFrom(itemType))
                            {
                                typeInfo.Add(p.Name, new SyntaxTreeMapBranchInfo(p, itemType, true, isCollection));
                                continue;
                            }
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
