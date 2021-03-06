﻿using ForgedOnce.Core;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.CSharp.Metadata
{
    public class CSharpNodePathService : INodePathService<SyntaxNode>
    {
        private readonly CodeFileCSharp codeFileCSharp;

        private SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor = new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>();

        public CSharpNodePathService(CodeFileCSharp codeFileCSharp)
        {
            this.codeFileCSharp = codeFileCSharp;
        }

        public ISingleNodeSnapshot GetSingleNodeSnapshot(SyntaxNode astNode)
        {
            var result = new SingleNodeSnapshot(this.codeFileCSharp, this.mappedVisitor);
            var annotatedRoot = result.Initialize(astNode);
            if (annotatedRoot != this.codeFileCSharp.SyntaxTree.GetRoot())
            {
                this.codeFileCSharp.SyntaxTree = annotatedRoot.SyntaxTree;
            }

            return result;
        }

        public ISubTreeSnapshot GetSubTreeSnapshot(SyntaxNode astNode)
        {
            var result = new SubTreeSnapshot(this.codeFileCSharp, this.mappedVisitor);
            var annotatedRoot = result.Initialize(astNode);
            if (annotatedRoot != this.codeFileCSharp.SyntaxTree.GetRoot())
            {
                this.codeFileCSharp.SyntaxTree = annotatedRoot.SyntaxTree;
            }

            return result;
        }

        public NodePath GetNodePath(SyntaxNode astNode)
        {
            var found = false;
            NodePath result = null;
            this.mappedVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        found = true;
                        result = new NodePath(Languages.CSharp, c.CurrentPath.Reverse());
                    }

                    return !found;
                });
            return result;
        }

        public SyntaxNode ResolveNode(NodePath nodePath)
        {
            SyntaxNode result = null;

            if (nodePath.Levels.Count > 0 && nodePath.Levels[0].Name == this.codeFileCSharp.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileCSharp.SyntaxTree.GetRoot(),
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }),
                (n, c) =>
                {
                    if (c.CurrentPath.Count == nodePath.Levels.Count)
                    {
                        result = n;
                    }

                    return result == null;
                },
                null,
                nodePath,
                1);
            }

            return result;
        }

        public NodePath ReplacePathRootWithThisFile(NodePath nodePath)
        {
            if (nodePath.Language != this.codeFileCSharp.Language)
            {
                throw new InvalidOperationException("Node path language does not correspond to this code file language.");
            }

            return new NodePath(nodePath.Language, new[] { new NodePathLevel(this.codeFileCSharp.Id, null) }.Concat(nodePath.Levels.Skip(1)));
        }
    }
}
