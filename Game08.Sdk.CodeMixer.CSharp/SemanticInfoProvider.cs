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

        public ISemanticSymbol GetSymbol(SyntaxNode astNode)
        {
            throw new NotImplementedException();
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
