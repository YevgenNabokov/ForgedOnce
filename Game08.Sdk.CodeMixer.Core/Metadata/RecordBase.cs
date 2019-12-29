using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public abstract class RecordBase
    {
        public RecordBase(int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
        {
            this.BatchIndex = batchindex;
            this.Scopes = new List<ISnapshot>();
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.PluginMetadata = pluginMetadata;
            this.Tags = new Dictionary<string, string>(tags);
        }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public object PluginMetadata { get; private set; }

        public int BatchIndex { get; private set; }

        public IDictionary<string, string> Tags { get; private set; }

        public List<ISnapshot> Scopes { get; private set; }
    }
}
