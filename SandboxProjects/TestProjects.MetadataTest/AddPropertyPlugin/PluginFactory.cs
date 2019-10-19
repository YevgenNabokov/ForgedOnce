using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Metadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<Settings, Metadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<Metadata> pluginPreprocessor = null)
        {
            return new Plugin()
            {
                Preprocessor = new Preprocessor()
            };
        }
    }
}
