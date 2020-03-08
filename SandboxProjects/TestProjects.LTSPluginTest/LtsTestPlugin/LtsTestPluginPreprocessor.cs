using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
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
