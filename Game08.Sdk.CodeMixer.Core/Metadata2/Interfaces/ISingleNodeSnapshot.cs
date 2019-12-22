using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces
{
    public interface ISingleNodeSnapshot : ISnapshot
    {
        MetadataRoot ResolveRoot();
    }
}
