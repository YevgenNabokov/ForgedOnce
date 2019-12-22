using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class PathHelperVisitor
    {
        public SyntaxNode Visit(SyntaxNode node, PathHelperVisitorContext context)
        {
            bool onSearchedNode = false;
            if (context.SearchedNode == node)
            {
                context.SearchedNodePath = new NodePath(Languages.CSharp, context.CurrentPath);
                context.IsOnOrBelowSearchedNode = true;
                onSearchedNode = true;
            }

            if (context.SearchedNodePath != null && !context.ShouldAnnotate())
            {
                return node;
            }
            
            SyntaxNode result = node;
            if (!(onSearchedNode && context.AnnotateOnlySearchedNode))
            {
                if (context.Map.ContainsKey(node.GetType()))
                {
                    foreach (var prop in context.Map[node.GetType()])
                    {
                        var propValue = prop.Value.Property.GetValue(result);
                        if (propValue != null)
                        {
                            if (prop.Value.IsCollection)
                            {
                                if (prop.Value.IsToken)
                                {
                                    var tokenCollection = propValue as IReadOnlyList<SyntaxToken>;
                                    for (var i = 0; i < tokenCollection.Count; i++)
                                    {
                                        context.CurrentPath.Push(new NodePathLevel(prop.Key, i));
                                        var itemResult = this.VisitToken(tokenCollection[i], context);
                                        context.CurrentPath.Pop();
                                        if (itemResult != tokenCollection[i])
                                        {
                                            result = result.ReplaceToken(tokenCollection[i], itemResult);
                                            tokenCollection = prop.Value.Property.GetValue(result) as IReadOnlyList<SyntaxToken>;
                                        }
                                    }
                                }
                                else
                                {
                                    var nodeCollection = propValue as IReadOnlyList<SyntaxNode>;
                                    for (var i = 0; i < nodeCollection.Count; i++)
                                    {
                                        context.CurrentPath.Push(new NodePathLevel(prop.Key, i));
                                        var itemResult = this.Visit(nodeCollection[i], context);
                                        context.CurrentPath.Pop();
                                        if (itemResult != nodeCollection[i])
                                        {
                                            result = result.ReplaceNode(nodeCollection[i], itemResult);
                                            nodeCollection = prop.Value.Property.GetValue(result) as IReadOnlyList<SyntaxNode>;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (prop.Value.IsToken)
                                {
                                    context.CurrentPath.Push(new NodePathLevel(prop.Key, null));
                                    var itemResult = this.VisitToken((SyntaxToken)propValue, context);
                                    context.CurrentPath.Pop();
                                    if (itemResult != (SyntaxToken)propValue)
                                    {
                                        result = result.ReplaceToken((SyntaxToken)propValue, itemResult);
                                    }
                                }
                                else
                                {
                                    context.CurrentPath.Push(new NodePathLevel(prop.Key, null));
                                    var itemResult = this.Visit((SyntaxNode)propValue, context);
                                    context.CurrentPath.Pop();
                                    if (itemResult != (SyntaxNode)propValue)
                                    {
                                        result = result.ReplaceNode((SyntaxNode)propValue, itemResult);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (context.ShouldAnnotate())
            {
                var pathAnnotation = new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(context.AnnotationId), NodePath.PartsToString(Languages.CSharp, context.CurrentPath));
                if (onSearchedNode)
                {
                    result = result.WithAdditionalAnnotations(
                        new SyntaxAnnotation(this.GetSearchedNodeAnnotationKey(context.AnnotationId), context.AnnotationId),
                        pathAnnotation);
                }
                else
                {
                    result = result.WithAdditionalAnnotations(pathAnnotation);
                }
            }

            if (onSearchedNode)
            {
                context.IsOnOrBelowSearchedNode = false;
            }

            return result;
        }

        private string GetSearchedNodeAnnotationKey(string annotationId)
        {
            return $"{annotationId}_ROOT";
        }

        private string GetOriginalPathAnnotationKey(string annotationId)
        {
            return $"{annotationId}_ORIGINAL_PATH";
        }

        public SyntaxToken VisitToken(SyntaxToken token, PathHelperVisitorContext context)
        {
            bool onSearchedNode = false;
            if (context.SearchedNode is SyntaxToken && (SyntaxToken)context.SearchedNode == token)
            {
                context.SearchedNodePath = new NodePath(Languages.CSharp, context.CurrentPath);
                context.IsOnOrBelowSearchedNode = true;
                onSearchedNode = true;
            }

            SyntaxToken result = token;

            if (context.ShouldAnnotate())
            {
                var pathAnnotation = new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(context.AnnotationId), NodePath.PartsToString(Languages.CSharp, context.CurrentPath));
                if (onSearchedNode)
                {
                    result = result.WithAdditionalAnnotations(
                        new SyntaxAnnotation(this.GetSearchedNodeAnnotationKey(context.AnnotationId), context.AnnotationId),
                        pathAnnotation);
                }
                else
                {
                    result = result.WithAdditionalAnnotations(pathAnnotation);
                }
            }

            if (onSearchedNode)
            {
                context.IsOnOrBelowSearchedNode = false;
            }

            return result;
        }
    }
}
