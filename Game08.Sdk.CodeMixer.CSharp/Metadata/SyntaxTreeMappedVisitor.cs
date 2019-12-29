using Game08.Sdk.CodeMixer.Core.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public class SyntaxTreeMappedVisitor<TContext> where TContext : SyntaxTreeMappedVisitorContext
    {
        public SyntaxNode Start(
            SyntaxNode syntaxNode,
            TContext context,
            Func<SyntaxNode, TContext, bool> onNodeEntry = null,
            Func<SyntaxNode, TContext, SyntaxNode> onNodeExit = null,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0
            )
        {
            return this.Visit(syntaxNode, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
        }

        private SyntaxNode Visit(
            SyntaxNode node,
            TContext context,
            Func<SyntaxNode, TContext, bool> onNodeEntry = null,
            Func<SyntaxNode, TContext, SyntaxNode> onNodeExit = null,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0)
        {            
            if  (onNodeEntry == null || onNodeEntry(node, context))
            {
                if (node != null)
                {
                    var nodeType = node.GetType();
                    if (context.Map.ContainsKey(nodeType))
                    {
                        var nodeTypeMap = context.Map[nodeType];
                        if (pathToFollow != null)
                        {
                            if (pathToFollow.Levels.Count > pathToFollowStartLevelIndex)
                            {
                                var nextLevel = pathToFollow.Levels[pathToFollowStartLevelIndex];
                                if (nodeTypeMap.ContainsKey(nextLevel.Name))
                                {
                                    var branch = nodeTypeMap[nextLevel.Name];
                                    node = this.VisitBranch(branch, node, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
                                }
                            }
                        }
                        else
                        {
                            foreach (var branch in nodeTypeMap.Values)
                            {
                                node = this.VisitBranch(branch, node, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
                            }
                        }
                    }
                }
            }

            if (onNodeExit != null)
            {
                return onNodeExit(node, context);
            }
            else
            {
                return node;
            }
        }

        private SyntaxNode VisitBranch(
            SyntaxTreeMapBranchInfo branch,
            SyntaxNode node,
            TContext context,
            Func<SyntaxNode, TContext, bool> onNodeEntry,
            Func<SyntaxNode, TContext, SyntaxNode> onNodeExit,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0)
        {
            var propValue = branch.Property.GetValue(node);
            if (propValue != null)
            {
                if (branch.IsCollection)
                {
                    if (!branch.IsToken)
                    {
                        var nodeCollection = propValue as IReadOnlyList<SyntaxNode>;
                        for (var i = 0; i < nodeCollection.Count; i++)
                        {
                            context.CurrentPath.Push(new NodePathLevel(branch.Property.Name, i));
                            var itemResult = this.Visit(nodeCollection[i], context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex + 1);
                            context.CurrentPath.Pop();
                            if (itemResult != nodeCollection[i])
                            {
                                node = node.ReplaceNode(nodeCollection[i], itemResult);
                                nodeCollection = branch.Property.GetValue(node) as IReadOnlyList<SyntaxNode>;
                            }
                        }
                    }
                }
                else
                {
                    if (!branch.IsToken)
                    {
                        context.CurrentPath.Push(new NodePathLevel(branch.Property.Name, null));
                        var itemResult = this.Visit((SyntaxNode)propValue, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex + 1);
                        context.CurrentPath.Pop();
                        if (itemResult != (SyntaxNode)propValue)
                        {
                            node = node.ReplaceNode((SyntaxNode)propValue, itemResult);
                        }
                    }
                }
            }

            return node;
        }
    }
}
