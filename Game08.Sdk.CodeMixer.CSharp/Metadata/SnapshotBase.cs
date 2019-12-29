using Game08.Sdk.CodeMixer.Core.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public abstract class SnapshotBase
    {
        protected readonly CodeFileCSharp codeFileCSharp;

        protected readonly SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor;

        protected NodePath originalRootPath;

        protected string annotationId;

        protected MetadataRoot[] roots;

        public SnapshotBase(
            CodeFileCSharp codeFileCSharp,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
        {
            this.codeFileCSharp = codeFileCSharp;
            this.mappedVisitor = mappedVisitor;
        }

        public string AnnotationId => this.annotationId;

        public SyntaxNode Initialize(SyntaxNode astNode)
        {
            SyntaxNode annotatedRoot = null;
            this.annotationId = Guid.NewGuid().ToString();
            this.originalRootPath = this.GetInitialRootPath(astNode, out annotatedRoot,!this.codeFileCSharp.IsReadOnly);
            if (this.codeFileCSharp.IsReadOnly)
            {
                this.roots = new[] { new MetadataRoot(this.originalRootPath, this.originalRootPath) };
            }

            return annotatedRoot;
        }

        protected abstract NodePath GetInitialRootPath(SyntaxNode astNode, out SyntaxNode annotatedRoot, bool annotate = true);        

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
