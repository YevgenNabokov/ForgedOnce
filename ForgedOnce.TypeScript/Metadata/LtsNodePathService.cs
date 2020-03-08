using ForgedOnce.Core;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript.Metadata
{
    public class LtsNodePathService : INodePathService<Node>
    {
        private readonly CodeFileLtsModel codeFileLts;

        private SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext> mappedVisitor = new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>();

        public LtsNodePathService(CodeFileLtsModel codeFileLts)
        {
            this.codeFileLts = codeFileLts;
        }

        public ISingleNodeSnapshot GetSingleNodeSnapshot(Node astNode)
        {
            var result = new SingleNodeSnapshot(this.codeFileLts, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public ISubTreeSnapshot GetSubTreeSnapshot(Node astNode)
        {
            var result = new SubTreeSnapshot(this.codeFileLts, this.mappedVisitor);
            result.Initialize(astNode);

            return result;
        }

        public NodePath GetNodePath(Node astNode)
        {
            var found = false;
            NodePath result = null;
            this.mappedVisitor.Start(
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
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

            if (nodePath.Levels.Count > 0 && nodePath.Levels[0].Name == this.codeFileLts.Id)
            {
                this.mappedVisitor.Start(
                this.codeFileLts.Model,
                new SyntaxTreeMappedVisitorContext(new[] { new NodePathLevel(this.codeFileLts.Id, null) }),
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
    }
}
