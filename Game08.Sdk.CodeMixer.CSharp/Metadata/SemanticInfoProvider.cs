using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public class SemanticInfoProvider : ISemanticInfoProvider<SyntaxNode>
    {
        private SemanticPathHelper semanticPathHelper;

        private readonly CodeFileCSharp codeFileCSharp;

        public SemanticInfoProvider(CodeFileCSharp codeFileCSharp)
        {
            this.codeFileCSharp = codeFileCSharp;
            this.semanticPathHelper = new SemanticPathHelper(codeFileCSharp);
        }
        public bool CanGetSymbolFor(SyntaxNode astNode)
        {
            return this.semanticPathHelper.CanGetExactPathFor(astNode);
        }

        public IEnumerable<ISemanticSymbol> GetImmediateDownstreamSymbols(SyntaxNode astNode)
        {
            List<ISemanticSymbol> result = new List<ISemanticSymbol>();

            foreach (var path in this.semanticPathHelper.GetImmediateDownstreamPaths(astNode))
            {
                result.Add(new SemanticSymbol(path));
            }

            return result;
        }

        public ISemanticSymbol GetImmediateUpstreamSymbol(SyntaxNode astNode)
        {
            var path = this.semanticPathHelper.GetImmediateUpstreamPath(astNode);
            if (path.Parts.Count == 0)
            {
                return null;
            }

            return new SemanticSymbol(path);
        }

        public ISemanticSymbol GetSymbolFor(SyntaxNode astNode)
        {
            var path = this.semanticPathHelper.GetExactPath(astNode);
            return new SemanticSymbol(path);
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
