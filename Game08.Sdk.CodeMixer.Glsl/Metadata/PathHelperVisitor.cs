using Game08.Sdk.GlslLanguageServices.LanguageModels;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata
{
    public class PathHelperVisitor : AstVisitor<PathHelperVisitorContext>
    {
        public IEnumerable<AstNode> FindDownstreamNodesWithGettableDirectPath(AstNode node)
        {
            PathHelperVisitorContext context = new PathHelperVisitorContext();

            this.Visit(node, context);

            return context.SearchResult;
        }

        public override void Visit(AstNode node, PathHelperVisitorContext context)
        {
            if (node != null && SemanticPathHelper.PathLevelTypeNames.ContainsKey(node.GetType()))
            {
                context.SearchResult.Add(node);
                return;
            }

            base.Visit(node, context);
        }
    }
}
