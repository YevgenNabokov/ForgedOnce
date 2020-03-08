using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;

namespace ForgedOnce.Glsl.Metadata
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
            var cSharpSyntaxNamespace = string.Join(".", nameof(ForgedOnce), nameof(ForgedOnce.GlslLanguageServices), nameof(ForgedOnce.GlslLanguageServices), nameof(ForgedOnce.GlslLanguageServices.LanguageModels), nameof(ForgedOnce.GlslLanguageServices.LanguageModels.Ast));
            Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> result = new Dictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>>();

            foreach (var type in typeof(AstNode).Assembly.GetTypes())
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

                            if (typeof(AstNode).IsAssignableFrom(itemType))
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
