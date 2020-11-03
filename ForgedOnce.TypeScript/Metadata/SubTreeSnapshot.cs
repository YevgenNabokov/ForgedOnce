using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.Core;
using System.Linq;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;

namespace ForgedOnce.TypeScript.Metadata
{
    public class SubTreeSnapshot : SnapshotBase, ISubTreeSnapshot
    {
        public SubTreeSnapshot(
            CodeFileTs codeFileTs,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileTs, mappedVisitor)
        {
        }

        protected override NodePath GetInitialRootPath(IStNode astNode, bool annotate = true)
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
                        nodePath = new NodePath(Languages.TypeScript, c.CurrentPath.Reverse());
                    }

                    return nodePath == null || (annotate && !leftRootNode);
                },
                (n, c) =>
                {
                    if (annotate && nodePath != null && c.CurrentPath.Count >= nodePath.Levels.Count)
                    {
                        var path = NodePath.PartsToString(Languages.TypeScript, c.CurrentPath.Reverse());
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

            Dictionary<IStNode, MetadataRoot> result = new Dictionary<IStNode, MetadataRoot>();
            Stack<IStNode> rootsStack = new Stack<IStNode>();

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
                            result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.TypeScript, c.CurrentPath.Reverse())));
                        }
                        else
                        {
                            var lastRoot = result[rootsStack.Peek()];
                            var currentPath = new NodePath(Languages.TypeScript, c.CurrentPath.Reverse());
                            if (!originalPath.StartsWith(lastRoot.OriginalPath)
                            || !currentPath.RelativeTo(lastRoot.CurrentPath).Equals(originalPath.RelativeTo(lastRoot.OriginalPath)))
                            {
                                rootsStack.Push(n);
                                result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.TypeScript, c.CurrentPath.Reverse())));
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
            if (!this.treeIsAnnotated)
            {
                if (this.originalRootPath != null)
                {
                    return path.StartsWith(this.originalRootPath);
                }

                throw new InvalidOperationException("Snapshot is not initialized.");
            }

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

        public NodePath GetNodeOriginalPath(NodePath currentPath)
        {
            if (!this.treeIsAnnotated)
            {
                if (this.originalRootPath != null)
                {
                    if (currentPath.StartsWith(this.originalRootPath))
                    {
                        return currentPath;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Path is not part of this snapshot scope. Use {nameof(ContainsNode)} method to check it.");
                    }
                }

                throw new InvalidOperationException("Snapshot is not initialized.");
            }

            NodePath result = null;
            var originalPathAnnotationKey = this.GetOriginalPathAnnotationKey();
            if (currentPath.Levels.Count > 0 && currentPath.Levels[0].Name == this.codeFileTs.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
                (n, c) =>
                {
                    if (c.CurrentPath.Count == currentPath.Levels.Count)
                    {
                        if (n.HasAnnotation(originalPathAnnotationKey))
                        {
                            result = NodePath.FromString(n.GetAnnotation(originalPathAnnotationKey));
                        }
                    }

                    return result == null;
                },
                null,
                currentPath,
                1);
            }

            return result;
        }
    }
}
