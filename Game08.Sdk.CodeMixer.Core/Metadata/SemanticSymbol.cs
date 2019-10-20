using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class SemanticSymbol : ISemanticSymbol
    {
        public SemanticSymbol(SemanticPath semanticPath)
        {
            this.SemanticPath = semanticPath;
        }

        public SemanticPath SemanticPath { get; private set; }
    }
}
