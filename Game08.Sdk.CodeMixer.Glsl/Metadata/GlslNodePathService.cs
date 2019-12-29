using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata
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
    }
}
