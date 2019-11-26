using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlslPlugin
{
    public class GlslTestPluginPreprocessor : IPluginPreprocessor<CodeFileCSharp, GlslTestPluginMetadata>
    {
        public GlslTestPluginMetadata GenerateMetadata(CodeFileCSharp input, IMetadataReader metadataReader, IFileGroup<CodeFileCSharp, GroupItemDetails> fileGroup = null)
        {
            return new GlslTestPluginMetadata();
        }
    }
}
