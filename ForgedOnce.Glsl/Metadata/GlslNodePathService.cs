﻿using ForgedOnce.Core;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Glsl.Metadata
{
    public class GlslNodePathService : INodePathService<AstNode>
    {
        private readonly CodeFileGlsl codeFileGlsl;

        private SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor = new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>();

        public GlslNodePathService(CodeFileGlsl codeFileGlsl)
        {
            this.codeFileGlsl = codeFileGlsl;
        }

        public ISingleNodeSnapshot GetSingleNodeSnapshot(AstNode astNode)
        {
            var result = new SingleNodeSnapshot(this.codeFileGlsl, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public ISubTreeSnapshot GetSubTreeSnapshot(AstNode astNode)
        {
            var result = new SubTreeSnapshot(this.codeFileGlsl, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public NodePath GetNodePath(AstNode astNode)
        {
            var found = false;
            NodePath result = null;
            this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        found = true;
                        result = new NodePath(Languages.Glsl, c.CurrentPath.Reverse());
                    }

                    return !found;
                });
            return result;
        }

        public AstNode ResolveNode(NodePath nodePath)
        {
            AstNode result = null;

            if (nodePath.Levels.Count > 0 && nodePath.Levels[0].Name == this.codeFileGlsl.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileGlsl.ShaderFile.SyntaxTree,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }),
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
            if (nodePath.Language != this.codeFileGlsl.Language)
            {
                throw new InvalidOperationException("Node path language does not correspond to this code file language.");
            }

            return new NodePath(nodePath.Language, new[] { new NodePathLevel(this.codeFileGlsl.Id, null) }.Concat(nodePath.Levels.Skip(1)));
        }
    }
}
