using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Modified : RecordBase
    {
        public Modified(ISemanticSymbol target, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Names.Add(target);
            this.Target = target;
        }

        public ISemanticSymbol Target { get; private set; }
    }
}
