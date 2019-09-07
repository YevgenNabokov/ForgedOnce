using Game08.Sdk.LTS.Builder;
using Game08.Sdk.LTS.Model.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers
{
    public class SearchVisitor : DefinitionTreeVisitor<SearchVisitorContext>
    {
        public IEnumerable<TNode> FindNodes<TNode>(Node node) where TNode: Node
        {
            SearchVisitorContext context = new SearchVisitorContext()
            {
                SearchedType = typeof(TNode)
            };

            this.Visit(node, context);

            return context.Result.Cast<TNode>();
        }

        public override void Visit(Node node, SearchVisitorContext context)
        {
            if (node != null && context.SearchedType != null && context.SearchedType.IsAssignableFrom(node.GetType()))
            {
                context.Result.Add(node);
            }

            base.Visit(node, context);
        }
    }
}
