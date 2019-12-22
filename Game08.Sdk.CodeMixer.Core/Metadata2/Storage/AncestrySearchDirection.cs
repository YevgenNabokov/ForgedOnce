using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    [Flags]
    public enum AncestrySearchDirection
    {
        None =      0b00,
        ToParent =  0b01,
        ToChild =   0b10,
        Both =      0b11
    }
}
