using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
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
