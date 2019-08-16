using Game08.Sdk.CodeMixer.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IMetadataWriter
    {
        void Write(RecordBase record);
    }
}
