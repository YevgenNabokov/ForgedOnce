using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Parameters, CodeFileCSharp>
    {
        public ICodeGenerationPlugin CreatePlugin(JObject configuration, IPipelineCreationContext context, IPluginPreprocessor<CodeFileCSharp, Parameters, Settings> pluginPreprocessor = null)
        {
            Settings settings = new Settings();
            if (configuration != null && configuration.ContainsKey(Settings.OutputNamespaceKey))
            {
                settings.OutputNamespace = configuration[Settings.OutputNamespaceKey].Value<string>();
            }

            return new Plugin()
            {
                Preprocessor = new Preprocessor(),
                Settings = settings
            };
        }
    }
}
