using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Generated : RecordBase
    {
        public Generated(ISemanticSymbol subject, ISemanticSymbol from = null)
        {
            this.Names.Add(subject);
            if (from != null)
            {
                this.Names.Add(from);
            }

            this.Subject = subject;
            this.From = from;
        }

        public ISemanticSymbol From { get; private set; }

        public ISemanticSymbol Subject { get; private set; }
    }
}
