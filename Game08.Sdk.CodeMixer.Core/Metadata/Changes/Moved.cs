using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Moved : RecordBase
    {
        public Moved(ISemanticSymbol from, ISemanticSymbol to)
        {
            this.Names.Add(from);
            this.Names.Add(to);
            this.From = from;
            this.To = to;
        }

        public ISemanticSymbol From { get; private set; }

        public ISemanticSymbol To { get; private set; }
    }
}
