﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game08.Sdk.LTS.Builder.DefinitionTree;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata2
{
    public class SingleNodeSnapshot : SnapshotBase, ISingleNodeSnapshot
    {
        public SingleNodeSnapshot(
            CodeFileLtsModel codeFileLts,
            SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor)
            : base(codeFileLts, mappedVisitor)
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
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
                (n, c) =>
                {
                    if (n.HasAnnotation(key))
                    {
                        currentPath = new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse());
                    }

                    return currentPath == null;
                });

            if (currentPath == null)
            {
                return null;
            }

            return new MetadataRoot(this.originalRootPath, currentPath);
        }

        protected override NodePath GetInitialRootPath(Node astNode, bool annotate = true)
        {
            this.annotationId = Guid.NewGuid().ToString();
            NodePath nodePath = null;
            this.mappedVisitor.Start(
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
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
                        n.SetAnnotation(this.GetOriginalPathAnnotationKey(), NodePath.PartsToString(Languages.LimitedTypeScript, c.CurrentPath.Reverse()));
                    }
                });
            
            return nodePath;
        }
    }
}
