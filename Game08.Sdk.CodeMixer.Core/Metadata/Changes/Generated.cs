using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Generated : RecordBase
    {
        public Generated(ISemanticName from, ISemanticName to)
        {
            this.Names.Add(from);
            this.Names.Add(to);
            this.From = from;
            this.To = to;
        }

        public ISemanticName From { get; private set; }

        public ISemanticName To { get; private set; }
    }
}
