using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class MetadataStore : IMetadataWriter, IMetadataReader
    {
        public string CurrentStageName;

        public string CurrentPluginId;

        public List<RecordBase> Records = new List<RecordBase>();        

        public void Write(RecordBase record)
        {
            this.Records.Add(record);
        }

        public void Refresh()
        {
            
        }
    }
}
