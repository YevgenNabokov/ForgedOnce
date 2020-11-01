using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ForgedOnce.TypeScript.Metadata
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
            var cSharpSyntaxNamespace = string.Join(".", nameof(ForgedOnce), nameof(ForgedOnce.TsLanguageServices), nameof(ForgedOnce.TsLanguageServices.FullSyntaxTree), nameof(ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel));
            Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> result = new Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>>();

            foreach (var type in typeof(StNode).Assembly.GetTypes())
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

                            if (typeof(StNode).IsAssignableFrom(itemType))
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
