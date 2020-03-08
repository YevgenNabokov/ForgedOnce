using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Glsl;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlslPlugin
{
    public class GlslTestPluginFactory : ICodeGenerationPluginFactory<GlslTestPluginSettings, GlslTestPluginMetadata, CodeFileCSharp>
    {

        ICodeGenerationPlugin ICodeGenerationPluginFactory<GlslTestPluginSettings, GlslTestPluginMetadata, CodeFileCSharp>.CreatePlugin(JObject configuration, IPluginPreprocessor<CodeFileCSharp, GlslTestPluginMetadata> pluginPreprocessor)
        {
            return new GlslTestPluginImplementation()
            {
                Preprocessor = new GlslTestPluginPreprocessor()
            };
        }
    }
}
