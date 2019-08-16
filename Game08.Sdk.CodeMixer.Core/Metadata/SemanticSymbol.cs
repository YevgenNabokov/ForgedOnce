using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class SemanticSymbol : ISemanticName
    {
        public string CodeFileId;

        public List<PathLevel> SemanticPath;

        public SemanticSymbol GetSymbol()
        {
            return this;
        }
    }
}
