using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Interfaces
{
    public interface ISemanticInfoProvider<TAstNode> : ISemanticInfoResolver
    {
        IEnumerable<ISemanticSymbol> GetImmediateDownstreamSymbols(TAstNode astNode);

        bool CanGetSymbolFor(TAstNode astNode);

        ISemanticSymbol GetSymbolFor(TAstNode astNode);

        ISemanticSymbol GetImmediateUpstreamSymbol(TAstNode astNode);
    }
}
