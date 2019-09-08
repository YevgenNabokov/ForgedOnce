using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LtsTestPlugin
{
    public class LtsTestPluginPreprocessor : IPluginPreprocessor<LtsTestPluginMetadata>
    {
        public LtsTestPluginMetadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new LtsTestPluginMetadata();
        }
    }
}
