using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using ForgedOnce.Core;
using System.Linq;

namespace ForgedOnce.TypeScript.Metadata
{
    public class SubTreeSnapshot : SnapshotBase, ISubTreeSnapshot
    {
        public SubTreeSnapshot(
            CodeFileTsModel codeFileTs,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileTs, mappedVisitor)
        {
        }

        protected override NodePath GetInitialRootPath(Node astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();

            NodePath nodePath = null;
            bool leftRootNode = false;
            this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
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
                        var path = NodePath.PartsToString(Languages.LimitedTypeScript, c.CurrentPath.Reverse());
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
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
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

        public bool ContainsNode(NodePath path)
        {
            bool result = false;
            var originalPathAnnotationKey = this.GetOriginalPathAnnotationKey();
            if (path.Levels.Count > 0 && path.Levels[0].Name == this.codeFileTs.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
                (n, c) =>
                {
                    if (c.CurrentPath.Count == path.Levels.Count)
                    {
                        result = n.HasAnnotation(originalPathAnnotationKey);
                    }

                    return result == false;
                },
                null,
                path,
                1);
            }

            return result;
        }
    }
}
