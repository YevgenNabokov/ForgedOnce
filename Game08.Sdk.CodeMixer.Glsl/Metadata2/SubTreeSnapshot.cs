using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata2
{
    public class SubTreeSnapshot : SnapshotBase, ISubTreeSnapshot
    {
        public SubTreeSnapshot(
            CodeFileGlsl codeFileGlsl,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileGlsl, mappedVisitor)
        {
        }

        protected override NodePath GetInitialRootPath(AstNode astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();

            NodePath nodePath = null;
            bool leftRootNode = false;
            this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.Glsl, c.CurrentPath.Reverse());
                    }

                    return nodePath == null || (annotate && !leftRootNode);
                },
                (n, c) =>
                {
                    if (annotate && nodePath != null && c.CurrentPath.Count >= nodePath.Levels.Count)
                    {
                        var path = NodePath.PartsToString(Languages.Glsl, c.CurrentPath.Reverse());
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

            Dictionary<AstNode, MetadataRoot> result = new Dictionary<AstNode, MetadataRoot>();
            Stack<AstNode> rootsStack = new Stack<AstNode>();

            this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotation(originalPathAnnotationKey))
                    {
                        var originalPath = NodePath.FromString(n.GetAnnotation(originalPathAnnotationKey));
                        if (rootsStack.Count == 0)
                        {
                            rootsStack.Push(n);
                            result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.Glsl, c.CurrentPath.Reverse())));
                        }
                        else
                        {
                            var lastRoot = result[rootsStack.Peek()];
                            var currentPath = new NodePath(Languages.Glsl, c.CurrentPath.Reverse());
                            if (!originalPath.StartsWith(lastRoot.OriginalPath)
                            || !currentPath.RelativeTo(lastRoot.CurrentPath).Equals(originalPath.RelativeTo(lastRoot.OriginalPath)))
                            {
                                rootsStack.Push(n);
                                result.Add(n, new MetadataRoot(originalPath, new NodePath(Languages.Glsl, c.CurrentPath.Reverse())));
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
