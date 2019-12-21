using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Changes
{
    public class Generated : RecordBase
    {
        public Generated(ISubTreeScope subject, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags, ISingleNodeScope from = null)
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

        public ISingleNodeScope From { get; private set; }

        public ISubTreeScope Subject { get; private set; }
    }
}
