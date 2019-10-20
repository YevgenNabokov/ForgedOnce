using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata
{
    public class SemanticPathHelper
    {
        public static string FileIdLevelType = "FileId";

        public static IReadOnlyDictionary<Type, string> PathLevelTypeNames = new ReadOnlyDictionary<Type, string>(new Dictionary<Type, string>()
        {
            { typeof(FunctionDeclaration), nameof(FunctionDeclaration) },
            { typeof(VariableDeclaration), nameof(VariableDeclaration) }
        });

        public static IReadOnlyDictionary<Type, Func<AstNode, string>> PathLevelIdentifierGetters = new ReadOnlyDictionary<Type, Func<AstNode, string>>(new Dictionary<Type, Func<AstNode, string>>()
        {
            { typeof(FunctionDeclaration), (n) => ((FunctionDeclaration)n).Name?.Name },
            { typeof(VariableDeclaration), (n) => ((VariableDeclaration)n).Name?.Name }
        });

        private readonly CodeFileGlsl codeFileGlsl;

        private PathHelperVisitor pathHelperVisitor = new PathHelperVisitor();

        public SemanticPathHelper(CodeFileGlsl codeFileGlsl)
        {
            this.codeFileGlsl = codeFileGlsl;
        }

        public bool CanGetExactPathFor(AstNode astNode)
        {
            return astNode != null && PathLevelTypeNames.ContainsKey(astNode.GetType());
        }

        public SemanticPath GetExactPath(AstNode astNode)
        {
            if (!this.CanGetExactPathFor(astNode))
            {
                throw new InvalidOperationException($"Cannot build exact semantic path for {astNode}.");
            }

            return this.GetImmediateUpstreamPath(astNode);
        }

        public IEnumerable<SemanticPath> GetImmediateDownstreamPaths(AstNode astNode)
        {
            List<SemanticPath> result = new List<SemanticPath>();

            foreach (var node in pathHelperVisitor.FindDownstreamNodesWithGettableDirectPath(astNode))
            {
                result.Add(this.GetExactPath(node));
            }

            return result;
        }

        public SemanticPath GetImmediateUpstreamPath(AstNode astNode)
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

            levels.Add(new PathLevel(this.codeFileGlsl.Id, FileIdLevelType));

            levels.Reverse();

            return new SemanticPath(Languages.Glsl, levels);
        }
    }
}
