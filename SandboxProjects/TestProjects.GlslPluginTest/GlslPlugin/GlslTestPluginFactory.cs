using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Glsl;
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
