using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Game08.Sdk.LTS.Builder.DefinitionTree;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
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
            var cSharpSyntaxNamespace = string.Join(".", nameof(Game08), nameof(Game08.Sdk), nameof(Game08.Sdk.LTS), nameof(Game08.Sdk.LTS.Builder), nameof(Game08.Sdk.LTS.Builder.DefinitionTree));
            Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> result = new Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>>();

            foreach (var type in typeof(Node).Assembly.GetTypes())
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

                            if (typeof(Node).IsAssignableFrom(itemType))
                            {
                                typeInfo.Add(p.Name, new SyntaxTreeMapBranchInfo(p, itemType, isCollection));
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
