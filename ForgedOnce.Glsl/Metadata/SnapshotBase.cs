using ForgedOnce.Core.Metadata;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Glsl.Metadata
{
    public abstract class SnapshotBase
    {
        protected readonly CodeFileGlsl codeFileGlsl;

        protected readonly SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor;

        protected NodePath originalRootPath;

        protected string annotationId;

        protected MetadataRoot[] roots;

        public SnapshotBase(
            CodeFileGlsl codeFileGlsl,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
        {
            this.codeFileGlsl = codeFileGlsl;
            this.mappedVisitor = mappedVisitor;
        }

        public string AnnotationId => this.annotationId;

        public void Initialize(AstNode astNode)
        {
            this.annotationId = Guid.NewGuid().ToString();
            this.originalRootPath = this.GetInitialRootPath(astNode, !this.codeFileGlsl.IsReadOnly);
            if (this.codeFileGlsl.IsReadOnly)
            {
                this.roots = new[] { new MetadataRoot(this.originalRootPath, this.originalRootPath) };
            }
        }

        protected abstract NodePath GetInitialRootPath(AstNode astNode, bool annotate = true);

        public string GetSnapshotRootAnnotationKey()
        {
            return $"{this.annotationId}_ROOT";
        }

        public string GetOriginalPathAnnotationKey()
        {
            return $"{this.annotationId}_ORIGINAL_PATH";
        }
    }
}
