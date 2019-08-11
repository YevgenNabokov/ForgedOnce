using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public abstract class PluginPreprocessor<TMetadata>
    {
        public abstract TMetadata GenerateMetadata(CodeFile input);
    }
}
