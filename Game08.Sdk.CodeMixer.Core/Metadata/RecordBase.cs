using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public abstract class RecordBase
    {
        public string StageName;

        public string PluginId;

        public object PluginMetadata;

        public HashSet<string> Tags;

        public List<ISemanticName> Names = new List<ISemanticName>();
    }
}
