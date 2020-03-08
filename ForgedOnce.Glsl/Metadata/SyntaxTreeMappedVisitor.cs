using ForgedOnce.Core.Metadata;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Glsl.Metadata
{
    public class SyntaxTreeMappedVisitor<TContext> where TContext : SyntaxTreeMappedVisitorContext
    {
        public void Start(
            AstNode syntaxNode,
            TContext context,
            Func<AstNode, TContext, bool> onNodeEntry = null,
            Action<AstNode, TContext> onNodeExit = null,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0
            )
        {
            this.Visit(syntaxNode, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
        }

        private void Visit(
            AstNode node,
            TContext context,
            Func<AstNode, TContext, bool> onNodeEntry = null,
            Action<AstNode, TContext> onNodeExit = null,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0)
        {
            if (onNodeEntry == null || onNodeEntry(node, context))
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
                                    this.VisitBranch(branch, node, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
                                }
                            }
                        }
                        else
                        {
                            foreach (var branch in nodeTypeMap.Values)
                            {
                                this.VisitBranch(branch, node, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
                            }
                        }
                    }
                }
            }

            onNodeExit?.Invoke(node, context);
        }

        private void VisitBranch(
            SyntaxTreeMapBranchInfo branch,
            AstNode node,
            TContext context,
            Func<AstNode, TContext, bool> onNodeEntry,
            Action<AstNode, TContext> onNodeExit,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0)
        {
            var propValue = branch.Property.GetValue(node);
            if (propValue != null)
            {
                if (branch.IsCollection)
                {
                    var nodeCollection = propValue as IReadOnlyList<AstNode>;
                    for (var i = 0; i < nodeCollection.Count; i++)
                    {
                        context.CurrentPath.Push(new NodePathLevel(branch.Property.Name, i));
                        this.Visit(nodeCollection[i], context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex + 1);
                        context.CurrentPath.Pop();
                    }
                }
                else
                {
                    context.CurrentPath.Push(new NodePathLevel(branch.Property.Name, null));
                    this.Visit((AstNode)propValue, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex + 1);
                    context.CurrentPath.Pop();
                }
            }
        }
    }
}
