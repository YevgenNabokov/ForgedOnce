using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using Game08.Sdk.CodeMixer.Core;
using System.Linq;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata2
{
    public class SubTreeSnapshot : SnapshotBase, ISubTreeSnapshot
    {
        public SubTreeSnapshot(
            CodeFileLtsModel codeFileLts,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileLts, mappedVisitor)
        {
        }

        protected override NodePath GetInitialRootPath(Node astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();

            NodePath nodePath = null;
            bool leftRootNode = false;
            this.mappedVisitor.Start(
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse());
                    }

                    return nodePath == null || (annotate && !leftRootNode);
                },
                (n, c) =>
                {
                    if (annotate && nodePath != null && c.CurrentPath.Count >= nodePath.Levels.Count)
                    {
                        var path = NodePath.PartsToString(Languages.CSharp, c.CurrentPath.Reverse());
                        if (nodePath.Levels.Count == c.CurrentPath.Count)
                        {
                            leftRootNode = true;
                            n.SetAnnotation(this.GetSnapshotRootAnnotationKey(), annotationId);
                            n.SetAnnotation(this.GetOriginalPathAnnotationKey(), path);
                        }
                        else
                        {
                            n.SetAnnotation(this.GetOriginalPathAnnotationKey(), path);
                        }
                    }
                });
            
            return nodePath;
        }

        public MetadataRoot[] ResolveRoots()
        {
            if (this.roots != null)
            {
                return this.roots;
            }

            var originalPathAnnotationKey = this.GetOriginalPathAnnotationKey();

            Dictionary<Node, MetadataRoot> result = new Dictionary<Node, MetadataRoot>();
            Stack<Node> rootsStack = new Stack<Node>();

            this.mappedVisitor.Start(
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotation(originalPathAnnotationKey))
                    {
                        var originalPath = NodePath.FromString(n.GetAnnotation(originalPathAnnotationKey));
                        if (rootsStack.Count == 0)
                        {
                            rootsStack.Push(n);
                            result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse())));
                        }
                        else
                        {
                            var lastRoot = result[rootsStack.Peek()];
                            var currentPath = new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse());
                            if (!originalPath.StartsWith(lastRoot.OriginalPath)
                            || !currentPath.RelativeTo(lastRoot.CurrentPath).Equals(originalPath.RelativeTo(lastRoot.OriginalPath)))
                            {
                                rootsStack.Push(n);
                                result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse())));
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
                });

            return result.Values.ToArray();
        }
    }
}
