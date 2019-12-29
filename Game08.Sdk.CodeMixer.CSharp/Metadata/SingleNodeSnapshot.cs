using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public class SingleNodeSnapshot : SnapshotBase, ISingleNodeSnapshot
    {
        public SingleNodeSnapshot(
            CodeFileCSharp codeFileCSharp,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileCSharp, mappedVisitor)
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
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotations(key))
                    {
                        currentPath = new NodePath(Languages.CSharp, c.CurrentPath.Reverse());
                    }                    

                    return currentPath == null;
                });

            if (currentPath == null)
            {
                return null;
            }

            return new MetadataRoot(this.originalRootPath, currentPath);
        }

        protected override NodePath GetInitialRootPath(SyntaxNode astNode, out SyntaxNode annotatedNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;            
            var root = this.mappedVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
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
                        return n.WithAdditionalAnnotations(
                                new SyntaxAnnotation(this.GetSnapshotRootAnnotationKey(), this.annotationId),
                                new SyntaxAnnotation(this.GetOriginalPathAnnotationKey(), NodePath.PartsToString(Languages.CSharp, c.CurrentPath.Reverse())));                        
                    }

                    return n;
                });

            annotatedNode = root;
            return nodePath;
        }
    }
}
