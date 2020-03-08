using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Changes
{
    public class Modified : RecordBase
    {
        public Modified(ISingleNodeSnapshot target, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Scopes.Add(target);
            this.Target = target;
        }

        public ISingleNodeSnapshot Target { get; private set; }
    }
}
