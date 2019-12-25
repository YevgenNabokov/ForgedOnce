using System;
using System.Collections.Generic;
using System.Text;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.LTS.Builder.DefinitionTree;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata2
{
    public class SyntaxTreeMappedVisitor<TContext> where TContext : SyntaxTreeMappedVisitorContext
    {
        public void Start(
            Node syntaxNode,
            TContext context,
            Func<Node, TContext, bool> onNodeEntry = null,
            Action<Node, TContext> onNodeExit = null,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0
            )
        {
            this.Visit(syntaxNode, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex);
        }

        private void Visit(
            Node node,
            TContext context,
            Func<Node, TContext, bool> onNodeEntry = null,
            Action<Node, TContext> onNodeExit = null,
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
            Node node,
            TContext context,
            Func<Node, TContext, bool> onNodeEntry,
            Action<Node, TContext> onNodeExit,
            NodePath pathToFollow = null,
            int pathToFollowStartLevelIndex = 0)
        {
            var propValue = branch.Property.GetValue(node);
            if (propValue != null)
            {
                if (branch.IsCollection)
                {
                    var nodeCollection = propValue as IReadOnlyList<Node>;
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
                    this.Visit((Node)propValue, context, onNodeEntry, onNodeExit, pathToFollow, pathToFollowStartLevelIndex + 1);
                    context.CurrentPath.Pop();
                }
            }
        }
    }
}
