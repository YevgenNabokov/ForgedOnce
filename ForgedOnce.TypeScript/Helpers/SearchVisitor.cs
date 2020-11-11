using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript.Helpers
{
    public class SearchVisitor
    {
        public IEnumerable<TNode> FindNodes<TNode>(StNodeBase node, Func<StNodeBase, bool> selector = null) where TNode: StNode
        {
            SearchVisitorContext context = new SearchVisitorContext()
            {
                SearchedType = typeof(TNode),
                Selector = selector
            };

            this.Visit(node, context);

            return context.Result.Cast<TNode>();
        }        

        public void Visit(StNodeBase node, SearchVisitorContext context)
        {
            if (node != null && context.SearchedType != null 
                && (context.SearchedType == null || context.SearchedType.IsAssignableFrom(node.GetType()))
                && (context.Selector == null || context.Selector(node)))
            {
                context.Result.Add(node);
            }

            foreach (var childNode in node.ChildNodes)
            {
                this.Visit(childNode, context);
            }
        }
    }
}
