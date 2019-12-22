using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Changes
{
    public class Generated : RecordBase
    {
        public Generated(ISubTreeSnapshot subject, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags, ISingleNodeSnapshot from = null)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Scopes.Add(subject);
            if (from != null)
            {
                this.Scopes.Add(from);
            }

            this.Subject = subject;
            this.From = from;
        }

        public ISingleNodeSnapshot From { get; private set; }

        public ISubTreeSnapshot Subject { get; private set; }
    }
}
