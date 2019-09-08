using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LtsTestPlugin
{
    public class LtsTestPluginFactory : ICodeGenerationPluginFactory<LtsTestPluginSettings, LtsTestPluginMetadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<LtsTestPluginSettings, LtsTestPluginMetadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<LtsTestPluginMetadata> pluginPreprocessor = null)
        {
            return new LtsTestPluginImplementation()
            {
                Preprocessor = new LtsTestPluginPreprocessor()
            };
        }
    }
}
