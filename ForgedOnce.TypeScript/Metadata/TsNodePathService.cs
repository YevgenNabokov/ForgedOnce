﻿using ForgedOnce.Core;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript.Metadata
{
    public class TsNodePathService : INodePathService<Node>
    {
        private readonly CodeFileTsModel codeFileTs;

        private SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor = new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>();

        public TsNodePathService(CodeFileTsModel codeFileTs)
        {
            this.codeFileTs = codeFileTs;
        }

        public ISingleNodeSnapshot GetSingleNodeSnapshot(Node astNode)
        {
            var result = new SingleNodeSnapshot(this.codeFileTs, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public ISubTreeSnapshot GetSubTreeSnapshot(Node astNode)
        {
            var result = new SubTreeSnapshot(this.codeFileTs, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public NodePath GetNodePath(Node astNode)
        {
            var found = false;
            NodePath result = null;
            this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
                (n, c) =>
                {
                    if (n == astNode)
                    {
                        found = true;
                        result = new NodePath(Languages.LimitedTypeScript, c.CurrentPath.Reverse());
                    }

                    return !found;
                });
            return result;
        }

        public Node ResolveNode(NodePath nodePath)
        {
            Node result = null;

            if (nodePath.Levels.Count > 0 && nodePath.Levels[0].Name == this.codeFileTs.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileTs.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileTs.Id, null) }),
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
            if (nodePath.Language != this.codeFileTs.Language)
            {
                throw new InvalidOperationException("Node path language does not correspond to this code file language.");
            }

            return new NodePath(nodePath.Language, new[] { new NodePathLevel(this.codeFileTs.Id, null) }.Concat(nodePath.Levels.Skip(1)));
        }
    }
}
