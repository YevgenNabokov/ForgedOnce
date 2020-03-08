using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
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
