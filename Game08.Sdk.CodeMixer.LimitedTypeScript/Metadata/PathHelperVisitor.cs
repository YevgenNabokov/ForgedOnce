using Game08.Sdk.LTS.Builder;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
{
    public class PathHelperVisitor : BuilderDefinitionTreeVisitor<PathHelperVisitorContext>
    {
        public IEnumerable<Node> FindDownstreamNodesWithGettableDirectPath(Node node)
        {
            PathHelperVisitorContext context = new PathHelperVisitorContext();

            this.Visit(node, context);

            return context.SearchResult;
        }

        public override void Visit(Node node, PathHelperVisitorContext context)
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
