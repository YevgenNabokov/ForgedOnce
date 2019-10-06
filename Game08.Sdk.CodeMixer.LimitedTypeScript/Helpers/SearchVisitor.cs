﻿using Game08.Sdk.LTS.Builder;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers
{
    public class SearchVisitor : BuilderDefinitionTreeVisitor<SearchVisitorContext>
    {
        public IEnumerable<TNode> FindNodes<TNode>(Node node, Func<Node, bool> selector = null) where TNode: Node
        {
            SearchVisitorContext context = new SearchVisitorContext()
            {
                SearchedType = typeof(TNode),
                Selector = selector
            };

            this.Visit(node, context);

            return context.Result.Cast<TNode>();
        }        

        public override void Visit(Node node, SearchVisitorContext context)
        {
            if (node != null && context.SearchedType != null 
                && (context.SearchedType == null || context.SearchedType.IsAssignableFrom(node.GetType()))
                && (context.Selector == null || context.Selector(node)))
            {
                context.Result.Add(node);
            }

            base.Visit(node, context);
        }
    }
}
