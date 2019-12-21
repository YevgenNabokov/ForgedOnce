using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Changes
{
    public class SourcingFrom : RecordBase
    {
        public SourcingFrom(ISingleNodeScope from, ISubTreeScope subject, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Scopes.Add(from);
            this.Scopes.Add(subject);
            this.From = from;
            this.Subject = subject;
        }

        public ISingleNodeScope From { get; private set; }

        public ISubTreeScope Subject { get; private set; }
    }
}
