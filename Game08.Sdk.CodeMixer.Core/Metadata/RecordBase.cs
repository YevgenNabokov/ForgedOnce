﻿using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public abstract class RecordBase
    {
        public RecordBase(string stageName, string pluginId, object pluginMetadata, HashSet<string> tags)
        {
            this.Names = new List<ISemanticSymbol>();
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.PluginMetadata = pluginMetadata;
            this.Tags = new HashSet<string>(tags);
        }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public object PluginMetadata { get; private set; }

        public HashSet<string> Tags { get; private set; }

        public List<ISemanticSymbol> Names { get; private set; }
    }
}
