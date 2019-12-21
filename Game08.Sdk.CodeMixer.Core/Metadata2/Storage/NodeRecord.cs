using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    public class NodeRecord
    {
        public NodeRecord(int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags, ChangeKind? change, NodeRelation[] relations)
        {
            this.BatchIndex = batchindex;
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.PluginMetadata = pluginMetadata;
            this.Tags = tags;
            this.Change = change;
            this.Relations = relations;
            if (relations != null)
            {
                foreach (var r in relations)
                r.ParentRecord = this;
            }
        }

        public int BatchIndex { get; private set; }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public object PluginMetadata { get; private set; }

        public IDictionary<string, string> Tags { get; private set; }

        public ChangeKind? Change { get; private set; }

        public NodeRelation[] Relations { get; private set; }
    }
}
