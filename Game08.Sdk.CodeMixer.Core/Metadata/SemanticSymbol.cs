using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class SemanticSymbol : ISemanticSymbol
    {
        public SemanticSymbol(int batchIndex, SemanticPath semanticPath)
        {
            this.BatchIndex = batchIndex;
            this.SemanticPath = semanticPath;
        }

        public int BatchIndex { get; private set; }

        public SemanticPath SemanticPath { get; private set; }
    }
}
