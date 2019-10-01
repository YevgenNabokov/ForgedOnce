using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class MetadataStore : IMetadataWriter, IMetadataReader
    {
        public List<RecordBase> Records = new List<RecordBase>();

        private Dictionary<int, MetadataIndex> metadataIndexed = new Dictionary<int, MetadataIndex>();

        public void Write(RecordBase record)
        {
            this.Records.Add(record);

            if (record is Bound)
            {
                var bound = (Bound)record;

                throw new NotImplementedException();

                return;
            }
        }

        public void Refresh()
        {
            
        }

        private Node AllocateNode(ISemanticSymbol symbol)
        {
            if (!this.metadataIndexed.ContainsKey(symbol.BatchIndex))
            {
                this.metadataIndexed.Add(symbol.BatchIndex, new MetadataIndex(symbol.BatchIndex));
            }

            return this.metadataIndexed[symbol.BatchIndex].AllocateNode(symbol.SemanticPath);
        }
    }
}
