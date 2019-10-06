using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.Metadata
{
    public class SemanticInfoProvider : ISemanticInfoProvider<AstNode>
    {
        private SemanticPathHelper semanticPathHelper;

        private readonly CodeFileGlsl codeFileGlsl;

        public SemanticInfoProvider(CodeFileGlsl codeFileGlsl)
        {
            this.codeFileGlsl = codeFileGlsl;
            this.semanticPathHelper = new SemanticPathHelper(codeFileGlsl);
        }

        public bool CanGetSymbolFor(AstNode astNode)
        {
            return this.semanticPathHelper.CanGetExactPathFor(astNode);
        }

        public IEnumerable<ISemanticSymbol> GetImmediateDownstreamSymbols(AstNode astNode)
        {
            List<ISemanticSymbol> result = new List<ISemanticSymbol>();

            foreach (var path in this.semanticPathHelper.GetImmediateDownstreamPaths(astNode))
            {
                result.Add(new SemanticSymbol(this.codeFileGlsl.LastRefreshBatchIndex, path));
            }

            return result;
        }

        public ISemanticSymbol GetImmediateUpstreamSymbol(AstNode astNode)
        {
            var path = this.semanticPathHelper.GetImmediateUpstreamPath(astNode);
            if (path.Parts.Count == 0)
            {
                return null;
            }

            return new SemanticSymbol(this.codeFileGlsl.LastRefreshBatchIndex, path);
        }

        public ISemanticSymbol GetSymbolFor(AstNode astNode)
        {
            var path = this.semanticPathHelper.GetExactPath(astNode);
            return new SemanticSymbol(this.codeFileGlsl.LastRefreshBatchIndex, path);
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
