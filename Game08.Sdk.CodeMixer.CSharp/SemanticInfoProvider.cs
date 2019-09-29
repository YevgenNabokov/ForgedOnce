using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class SemanticInfoProvider : ISemanticInfoProvider<SyntaxNode>
    {
        private readonly CodeFileCSharp codeFileCSharp;

        public SemanticInfoProvider(CodeFileCSharp codeFileCSharp)
        {
            this.codeFileCSharp = codeFileCSharp;
        }
        public bool CanGetSymbolFor(SyntaxNode astNode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISemanticSymbol> GetImmediateDownstreamSymbols(SyntaxNode astNode)
        {
            throw new NotImplementedException();
        }

        public ISemanticSymbol GetImmediateUpstreamSymbol(SyntaxNode astNode)
        {
            throw new NotImplementedException();
        }

        public ISemanticSymbol GetSymbolFor(SyntaxNode astNode)
        {
            throw new NotImplementedException();
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
