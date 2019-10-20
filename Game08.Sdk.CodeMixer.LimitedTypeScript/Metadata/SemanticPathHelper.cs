using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
{
    public class SemanticPathHelper
    {
        public static string FileIdLevelType = "FileId";

        public static IReadOnlyDictionary<Type, string> PathLevelTypeNames = new ReadOnlyDictionary<Type, string>(new Dictionary<Type, string>()
        {
            { typeof(ClassDefinition), nameof(ClassDefinition) },
            { typeof(InterfaceDefinition), nameof(InterfaceDefinition) },
            { typeof(EnumDefinition), nameof(EnumDefinition) },
            { typeof(FieldDeclaration), nameof(FieldDeclaration) },
            { typeof(MethodDeclaration), nameof(MethodDeclaration) },
            { typeof(PropertyDeclaration), nameof(PropertyDeclaration) },
            { typeof(EnumMember), nameof(EnumMember) }
        });

        public static IReadOnlyDictionary<Type, Func<Node, string>> PathLevelIdentifierGetters = new ReadOnlyDictionary<Type, Func<Node, string>>(new Dictionary<Type, Func<Node, string>>()
        {
            { typeof(ClassDefinition), (n) => ((ClassDefinition)n).Name?.Name },
            { typeof(InterfaceDefinition), (n) => ((InterfaceDefinition)n).Name?.Name },
            { typeof(EnumDefinition), (n) => ((EnumDefinition)n).Name?.Name },
            { typeof(FieldDeclaration), (n) => ((FieldDeclaration)n).Name?.Name },
            { typeof(MethodDeclaration), (n) => ((MethodDeclaration)n).Name?.Name },
            { typeof(PropertyDeclaration), (n) => ((PropertyDeclaration)n).Name?.Name },
            { typeof(EnumMember), (n) => ((EnumMember)n).Name?.Name }
        });

        private readonly CodeFileLtsModel codeFileLtsModel;

        private PathHelperVisitor pathHelperVisitor = new PathHelperVisitor();

        public SemanticPathHelper(CodeFileLtsModel codeFileLtsModel)
        {
            this.codeFileLtsModel = codeFileLtsModel;
        }

        public bool CanGetExactPathFor(Node astNode)
        {
            return astNode != null && PathLevelTypeNames.ContainsKey(astNode.GetType());
        }

        public SemanticPath GetExactPath(Node astNode)
        {
            if (!this.CanGetExactPathFor(astNode))
            {
                throw new InvalidOperationException($"Cannot build exact semantic path for {astNode}.");
            }

            return this.GetImmediateUpstreamPath(astNode);
        }

        public IEnumerable<SemanticPath> GetImmediateDownstreamPaths(Node astNode)
        {
            List<SemanticPath> result = new List<SemanticPath>();

            foreach (var node in pathHelperVisitor.FindDownstreamNodesWithGettableDirectPath(astNode))
            {
                result.Add(this.GetExactPath(node));
            }

            return result;
        }

        public SemanticPath GetImmediateUpstreamPath(Node astNode)
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

            levels.Add(new PathLevel(this.codeFileLtsModel.Id, FileIdLevelType));

            levels.Reverse();

            return new SemanticPath(Languages.LimitedTypeScript, levels);
        }
    }
}
