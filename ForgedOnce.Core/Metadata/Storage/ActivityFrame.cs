using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Storage
{
    public class ActivityFrame
    {
        public ActivityFrame(string stageName = null, string pluginId = null, int? batchIndex = null)
        {
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.BatchIndex = batchIndex;
        }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public int? BatchIndex { get; private set; }
    }
}
