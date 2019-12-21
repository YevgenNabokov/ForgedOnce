using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces
{
    public interface ISingleNodeScope : IScope
    {
        MetadataRoot ResolveRoot();
    }
}
