using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Core.Metadata.Storage
{
    public class ActivityFrame
    {
        public ActivityFrame(string stageName = null, string pluginId = null, int? batchIndex = null, TagSelector tagSelector = null)
        {
            this.StageName = stageName;
            this.PluginId = pluginId;
            this.BatchIndex = batchIndex;
            this.TagSelector = tagSelector;
        }

        public string StageName { get; private set; }

        public string PluginId { get; private set; }

        public int? BatchIndex { get; private set; }

        public TagSelector TagSelector { get; private set; }
    }
}
