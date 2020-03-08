using ForgedOnce.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;

namespace ForgedOnce.TypeScript.Metadata
{
    public abstract class SnapshotBase
    {
        protected readonly CodeFileTsModel codeFileTs;

        protected readonly SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor;

        protected NodePath originalRootPath;

        protected string annotationId;

        protected MetadataRoot[] roots;

        public SnapshotBase(
            CodeFileTsModel codeFileTs,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
        {
            this.codeFileTs = codeFileTs;
            this.mappedVisitor = mappedVisitor;
        }

        public string AnnotationId => this.annotationId;

        public void Initialize(Node astNode)
        {
            this.annotationId = Guid.NewGuid().ToString();
            this.originalRootPath = this.GetInitialRootPath(astNode, !this.codeFileTs.IsReadOnly);
            if (this.codeFileTs.IsReadOnly)
            {
                this.roots = new[] { new MetadataRoot(this.originalRootPath, this.originalRootPath) };
            }
        }

        protected abstract NodePath GetInitialRootPath(Node astNode, bool annotate = true);

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
