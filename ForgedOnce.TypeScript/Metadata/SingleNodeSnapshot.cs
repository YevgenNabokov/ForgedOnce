using ForgedOnce.Core;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript.Metadata
{
    public class SingleNodeSnapshot : SnapshotBase, ISingleNodeSnapshot
    {
        public SingleNodeSnapshot(
            CodeFileTs codeFileTs,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileTs, mappedVisitor)
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
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotation(key))
                    {
                        currentPath = new NodePath(Languages.TypeScript, c.CurrentPath.Reverse());
                    }

                    return currentPath == null;
                });

            if (currentPath == null)
            {
                return null;
            }

            return new MetadataRoot(this.originalRootPath, currentPath);
        }

        protected override NodePath GetInitialRootPath(IStNode astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;
            this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        nodePath = new NodePath(Languages.TypeScript, c.CurrentPath.Reverse());
                    }

                    return nodePath == null;
                },
                (n, c) =>
                {
                    if (annotate && n == astNode)
                    {
                        n.SetAnnotation(this.GetSnapshotRootAnnotationKey(), this.annotationId);
                        n.SetAnnotation(this.GetOriginalPathAnnotationKey(), NodePath.PartsToString(Languages.TypeScript, c.CurrentPath.Reverse()));
                    }
                });
            
            return nodePath;
        }
    }
}
