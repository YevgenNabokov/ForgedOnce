using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Removed : RecordBase
    {
        public Removed(ISemanticSymbol target, string stageName, string pluginId, object pluginMetadata, HashSet<string> tags)
            : base(stageName, pluginId, pluginMetadata, tags)
        {
            this.Names.Add(target);
            this.Target = target;
        }

        public ISemanticSymbol Target { get; private set; }
    }
}
