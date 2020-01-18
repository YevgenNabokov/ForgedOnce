using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Helpers.SemanticAnalysis
{
    public static class TypeSymbolExtensions
    {
        public static bool InheritsFromOrImplementsOrEqualsIgnoringConstruction(this ITypeSymbol type, ITypeSymbol baseType)
        {
            var originalBaseType = baseType.OriginalDefinition;
            type = type.OriginalDefinition;

            if (type.GetFullMetadataName() == originalBaseType.GetFullMetadataName())
            {
                return true;
            }

            IEnumerable<ITypeSymbol> baseTypes = (baseType.TypeKind == TypeKind.Interface) ? type.AllInterfaces : type.GetBaseTypes();
            return baseTypes.Any(t => t.OriginalDefinition.GetFullMetadataName() == originalBaseType.GetFullMetadataName());
        }

        public static IEnumerable<INamedTypeSymbol> GetBaseTypes(this ITypeSymbol type)
        {
            var current = type.BaseType;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static string GetFullMetadataName(this ITypeSymbol symbol)
        {
            return $"{symbol.ContainingNamespace.ToDisplayString()}.{symbol.MetadataName}";
        }

        public static IEnumerable<TSymbol> GetAllSymbols<TSymbol>(this ITypeSymbol symbol, SymbolKind kind, Accessibility accessibility, bool includeBaseTypes = true, ITypeSymbol breakOnType = null) where TSymbol : ISymbol
        {
            if (breakOnType == null || !SymbolEqualityComparer.Default.Equals(symbol, breakOnType))
            {
                foreach (var p in symbol
                    .GetMembers()
                    .Where(m => m.DeclaredAccessibility == accessibility  && m.Kind == kind)
                    .OfType<TSymbol>())
                {
                    yield return p;
                }

                if (includeBaseTypes && symbol.BaseType != null)
                {
                    foreach (var p in symbol.BaseType.GetAllSymbols<TSymbol>(kind, accessibility, includeBaseTypes, breakOnType))
                    {
                        yield return p;
                    }
                }
            }
        }
    }
}
