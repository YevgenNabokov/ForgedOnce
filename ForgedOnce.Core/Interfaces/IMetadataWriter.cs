using ForgedOnce.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IMetadataWriter
    {
        void Write(RecordBase record);
    }
}
