using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class DummySemanticInfoResolver : ISemanticInfoResolver
    {
        public bool Resolve(ISemanticSymbol symbol)
        {
            return false;
        }
    }
}
