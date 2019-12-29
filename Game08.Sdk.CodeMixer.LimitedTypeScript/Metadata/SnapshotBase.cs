using Game08.Sdk.CodeMixer.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using Game08.Sdk.LTS.Builder.DefinitionTree;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
{
    public abstract class SnapshotBase
    {
        protected readonly CodeFileLtsModel codeFileLts;

        protected readonly SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor;

        protected NodePath originalRootPath;

        protected string annotationId;

        protected MetadataRoot[] roots;

        public SnapshotBase(
            CodeFileLtsModel codeFileLts,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
        {
            this.codeFileLts = codeFileLts;
            this.mappedVisitor = mappedVisitor;
        }

        public string AnnotationId => this.annotationId;

        public void Initialize(Node astNode)
        {
            this.annotationId = Guid.NewGuid().ToString();
            this.originalRootPath = this.GetInitialRootPath(astNode, !this.codeFileLts.IsReadOnly);
            if (this.codeFileLts.IsReadOnly)
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
