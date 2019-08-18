using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class MetadataStore : IMetadataWriter, IMetadataReader
    {
        public string CurrentStageName;

        public string CurrentPluginId;

        public List<RecordBase> Records = new List<RecordBase>();

        public List<IResolvableName> UnresolvedNames = new List<IResolvableName>();

        public void Write(RecordBase record)
        {
            this.Records.Add(record);

            foreach (var name in record.Names)
            {
                IResolvableName resolvable = name as IResolvableName;
                if (resolvable != null)
                {
                    this.UnresolvedNames.Add(resolvable);
                }
            }
        }

        public void ResolveNames()
        {
            foreach (var name in this.UnresolvedNames)
            {
                name.Resolve();
            }

            this.UnresolvedNames.Clear();
        }
    }
}
