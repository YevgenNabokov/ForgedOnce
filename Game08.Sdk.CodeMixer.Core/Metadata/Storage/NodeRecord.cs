using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class NodeRecord
    {
        public NodeRecord(string stageName, string pluginId, object pluginMetadata, HashSet<string> tags, ChangeKind? change, NodeRelation relation)
        {
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.PluginMetadata = pluginMetadata;
            this.Tags = tags;
            this.Change = change;
            this.Relation = relation;
        }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public object PluginMetadata { get; private set; }

        public HashSet<string> Tags { get; private set; }

        public ChangeKind? Change { get; private set; }

        public NodeRelation Relation { get; private set; }
    }
}
