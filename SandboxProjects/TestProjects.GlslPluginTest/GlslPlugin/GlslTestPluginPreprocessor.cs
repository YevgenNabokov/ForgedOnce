using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlslPlugin
{
    public class GlslTestPluginPreprocessor : IPluginPreprocessor<GlslTestPluginMetadata>
    {
        public GlslTestPluginMetadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new GlslTestPluginMetadata();
        }
    }
}
