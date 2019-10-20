using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class SourcingFrom : RecordBase
    {
        public SourcingFrom(ISemanticSymbol from, ISemanticSymbol subject, int batchindex, string stageName, string pluginId, object pluginMetadata, HashSet<string> tags)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Names.Add(from);
            this.Names.Add(subject);
            this.From = from;
            this.Subject = subject;
        }

        public ISemanticSymbol From { get; private set; }

        public ISemanticSymbol Subject { get; private set; }
    }
}
