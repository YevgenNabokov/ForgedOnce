using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata
{
    public class SingleNodeSnapshot : SnapshotBase, ISingleNodeSnapshot
    {
        public SingleNodeSnapshot(
            CodeFileGlsl codeFileGlsl,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileGlsl, mappedVisitor)
        {
        }

        public MetadataRoot ResolveRoot()
        {
            if (this.roots != null && this.roots.Length > 0)
            {
                return this.roots[0];
            }

            var key = this.GetSnapshotRootAnnotationKey();
            NodePath currentPath = null;
            this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotation(key))
                    {
                        currentPath = new NodePath(Languages.Glsl, c.CurrentPath.Reverse());
                    }

                    return currentPath == null;
                });

            if (currentPath == null)
            {
                return null;
            }

            return new MetadataRoot(this.originalRootPath, currentPath);
        }

        protected override NodePath GetInitialRootPath(AstNode astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;
            this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.CSharp, c.CurrentPath.Reverse());
                    }

                    return nodePath == null;
                },
                (n, c) =>
                {
                    if (annotate && n == astNode)
                    {
                        n.SetAnnotation(this.GetSnapshotRootAnnotationKey(), this.annotationId);
                        n.SetAnnotation(this.GetOriginalPathAnnotationKey(), NodePath.PartsToString(Languages.Glsl, c.CurrentPath.Reverse()));
                    }
                });

            return nodePath;
        }
    }
}
