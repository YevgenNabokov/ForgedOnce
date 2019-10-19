using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public class SemanticPathHelper
    {
        public static string FileIdLevelType = "FileId";

        public static IReadOnlyDictionary<Type, string> PathLevelTypeNames = new ReadOnlyDictionary<Type, string>(new Dictionary<Type, string>()
        {
            { typeof(NamespaceDeclarationSyntax), nameof(NamespaceDeclarationSyntax) },
            { typeof(ClassDeclarationSyntax), nameof(ClassDeclarationSyntax) },
            { typeof(InterfaceDeclarationSyntax), nameof(InterfaceDeclarationSyntax) },
            { typeof(EnumDeclarationSyntax), nameof(EnumDeclarationSyntax) },
            { typeof(MethodDeclarationSyntax), nameof(MethodDeclarationSyntax) },
            { typeof(PropertyDeclarationSyntax), nameof(PropertyDeclarationSyntax) },
            { typeof(VariableDeclaratorSyntax), nameof(VariableDeclaratorSyntax) },
        });

        public static IReadOnlyDictionary<Type, Func<SyntaxNode, string>> PathLevelIdentifierGetters = new ReadOnlyDictionary<Type, Func<SyntaxNode, string>>(new Dictionary<Type, Func<SyntaxNode, string>>()
        {
            { typeof(NamespaceDeclarationSyntax), (n) => GetName(((NamespaceDeclarationSyntax)n).Name) },
            { typeof(ClassDeclarationSyntax), (n) => ((ClassDeclarationSyntax)n).Identifier.ValueText },
            { typeof(InterfaceDeclarationSyntax), (n) => ((InterfaceDeclarationSyntax)n).Identifier.ValueText },
            { typeof(EnumDeclarationSyntax), (n) => ((EnumDeclarationSyntax)n).Identifier.ValueText },
            { typeof(MethodDeclarationSyntax), (n) => ((MethodDeclarationSyntax)n).Identifier.ValueText },
            { typeof(PropertyDeclarationSyntax), (n) => ((PropertyDeclarationSyntax)n).Identifier.ValueText },
            { typeof(VariableDeclaratorSyntax), (n) => ((VariableDeclaratorSyntax)n).Identifier.ValueText },
        });

        private static string GetName(NameSyntax nameSyntax)
        {
            if (nameSyntax == null)
            {
                return null;
            }

            if (nameSyntax is IdentifierNameSyntax)
            {
                return (nameSyntax as IdentifierNameSyntax).Identifier.ValueText;
            }

            if (nameSyntax is QualifiedNameSyntax)
            {
                return (nameSyntax as QualifiedNameSyntax).GetText().ToString();
            }

            throw new NotSupportedException($"{nameSyntax.GetType()} is not supported as name for syntax path.");
        }

        private readonly CodeFileCSharp codeFileCSharp;

        public SemanticPathHelper(CodeFileCSharp codeFileCSharp)
        {
            this.codeFileCSharp = codeFileCSharp;
        }

        public bool CanGetExactPathFor(SyntaxNode astNode)
        {
            return astNode != null && PathLevelTypeNames.ContainsKey(astNode.GetType());
        }

        public SemanticPath GetExactPath(SyntaxNode astNode)
        {
            if (!this.CanGetExactPathFor(astNode))
            {
                throw new InvalidOperationException($"Cannot build exact semantic path for {astNode}.");
            }

            return this.GetImmediateUpstreamPath(astNode);
        }

        public IEnumerable<SemanticPath> GetImmediateDownstreamPaths(SyntaxNode astNode)
        {
            List<SemanticPath> result = new List<SemanticPath>();

            List<SyntaxNode> currentBatch = new List<SyntaxNode>();
            List<SyntaxNode> nextBatch = new List<SyntaxNode>();
            currentBatch.Add(astNode);

            while (currentBatch.Count > 0)
            {
                foreach (var node in currentBatch)
                {
                    if (PathLevelTypeNames.ContainsKey(node.GetType()))
                    {
                        result.Add(this.GetExactPath(node));
                    }
                    else
                    {
                        nextBatch.AddRange(node.ChildNodes());
                    }
                }

                var b = currentBatch;
                currentBatch = nextBatch;
                nextBatch = b;
                nextBatch.Clear();
            }

            return result;
        }

        public SemanticPath GetImmediateUpstreamPath(SyntaxNode astNode)
        {
            var levels = new List<PathLevel>();

            var node = astNode;
            while (node != null)
            {
                if (PathLevelTypeNames.ContainsKey(node.GetType()))
                {
                    levels.Add(new PathLevel(PathLevelIdentifierGetters[node.GetType()](node), PathLevelTypeNames[node.GetType()]));
                }

                node = node.Parent;
            }

            levels.Add(new PathLevel(this.codeFileCSharp.Id, FileIdLevelType));

            levels.Reverse();

            return new SemanticPath(Languages.CSharp, levels);
        }
    }
}
