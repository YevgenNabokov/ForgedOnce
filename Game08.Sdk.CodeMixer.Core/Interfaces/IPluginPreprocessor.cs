using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IPluginPreprocessor<TMetadata>
    {
        TMetadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader);
    }
}
