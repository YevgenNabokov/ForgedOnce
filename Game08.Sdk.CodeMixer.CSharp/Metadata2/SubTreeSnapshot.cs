using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class SubTreeSnapshot : SnapshotBase, ISubTreeSnapshot
    {        
        public SubTreeSnapshot(
            CodeFileCSharp codeFileCSharp,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            :base(codeFileCSharp, mappedVisitor)
        {
        }

        protected override NodePath GetInitialRootPath(SyntaxNode astNode, out SyntaxNode annotatedRoot, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();

            NodePath nodePath = null;            
            bool leftRootNode = false;
            var root = this.mappedVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.CSharp, c.CurrentPath.Reverse());
                    }

                    return nodePath == null || (annotate && !leftRootNode);
                },
                (n, c) =>
                {
                    if (annotate && nodePath != null && c.CurrentPath.Count >= nodePath.Levels.Count)
                    {
                        var pathAnnotation = new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(), NodePath.PartsToString(Languages.CSharp, c.CurrentPath.Reverse()));
                        if (nodePath.Levels.Count == c.CurrentPath.Count)
                        {
                            leftRootNode = true;
                            return n.WithAdditionalAnnotations(
                                new SyntaxAnnotation(this.GetSnapshotRootAnnotationKey(), annotationId),
                                pathAnnotation);                            
                        }
                        else
                        {
                            return n.WithAdditionalAnnotations(pathAnnotation);
                        }
                    }

                    return n;
                });

            annotatedRoot = root;
            return nodePath;
        }

        public MetadataRoot[] ResolveRoots()
        {
            if (this.roots != null)
            {
                return this.roots;
            }

            var originalPathAnnotationKey = this.GetOriginalPathAnnotationKey();

            Dictionary<SyntaxNode, MetadataRoot> result = new Dictionary<SyntaxNode, MetadataRoot>();
            Stack<SyntaxNode> rootsStack = new Stack<SyntaxNode>();

            this.mappedVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotations(originalPathAnnotationKey))
                    {
                        var originalPath = NodePath.FromString(n.GetAnnotations(originalPathAnnotationKey).First().Data);
                        if (rootsStack.Count == 0)
                        {
                            rootsStack.Push(n);
                            result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.CSharp, c.CurrentPath.Reverse())));
                        }
                        else
                        {
                            var lastRoot = result[rootsStack.Peek()];
                            var currentPath = new NodePath(Languages.CSharp, c.CurrentPath.Reverse());
                            if (!originalPath.StartsWith(lastRoot.OriginalPath)
                            || !currentPath.RelativeTo(lastRoot.CurrentPath).Equals(originalPath.RelativeTo(lastRoot.OriginalPath)))
                            {
                                rootsStack.Push(n);
                                result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.CSharp, c.CurrentPath.Reverse())));
                            }
                        }
                    }

                    return true;
                },
                (n, c) =>
                {
                    if (rootsStack.Count > 0 && rootsStack.Peek() == n)
                    {
                        rootsStack.Pop();
                    }

                    return n;
                });

            return result.Values.ToArray();
        }
    }
}
