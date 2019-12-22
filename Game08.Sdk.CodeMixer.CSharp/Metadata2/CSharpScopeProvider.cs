using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class CSharpScopeProvider : IScopeProvider<SyntaxNode>
    {
        private readonly CodeFileCSharp codeFileCSharp;

        private SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> pathHelperVisitor = new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>();

        public CSharpScopeProvider(CodeFileCSharp codeFileCSharp)
        {
            this.codeFileCSharp = codeFileCSharp;
        }

        public bool IsSyntaxTreeFrozen()
        {
            return this.codeFileCSharp.IsReadOnly;
        }

        public ISingleNodeScope GetSingleNodeScope(SyntaxNode astNode)
        {
            var annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;
            this.pathHelperVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.CSharp, c.CurrentPath);
                    }

                    return nodePath == null;
                },
                (n, c) => 
                {
                    if (n == astNode)
                    {
                        return n.WithAdditionalAnnotations(
                                new SyntaxAnnotation(this.GetSearchedNodeAnnotationKey(annotationId), annotationId),
                                new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(annotationId), NodePath.PartsToString(Languages.CSharp, c.CurrentPath)));
                    }

                    return n;
                });
            

            throw new NotImplementedException();
        }

        public ISubTreeScope GetSubTreeScope(SyntaxNode astNode)
        {
            var annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;
            this.pathHelperVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.CSharp, c.CurrentPath);
                    }

                    return nodePath == null || c.CurrentPath.Count > nodePath.Levels.Count;
                },
                (n, c) =>
                {
                    if (nodePath != null)
                    {
                        var pathAnnotation = new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(annotationId), NodePath.PartsToString(Languages.CSharp, c.CurrentPath));
                        if (n == astNode)
                        {
                            return n.WithAdditionalAnnotations(
                                new SyntaxAnnotation(this.GetSearchedNodeAnnotationKey(annotationId), annotationId),
                                pathAnnotation);
                        }
                        else
                        {
                            return n.WithAdditionalAnnotations(pathAnnotation);
                        }
                    }

                    return n;
                });

            throw new NotImplementedException();
        }

        public NodePath GetNodePath(SyntaxNode astNode)
        {
            var found = false;
            NodePath result = null;
            this.pathHelperVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        found = true;
                        result = new NodePath(Languages.CSharp, c.CurrentPath);
                    }

                    return found;
                });
            return result;
        }

        public SyntaxNode ResolveNode(NodePath nodePath)
        {
            SyntaxNode result = null;

            if (nodePath.Levels.Count > 0 && nodePath.Levels[0].Name == this.codeFileCSharp.Id)
            {
                this.pathHelperVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (c.CurrentPath.Count == nodePath.Levels.Count - 1)
                    {
                        result = n;
                    }

                    return result == null;
                },
                null,
                nodePath,
                1);
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
    }
}
