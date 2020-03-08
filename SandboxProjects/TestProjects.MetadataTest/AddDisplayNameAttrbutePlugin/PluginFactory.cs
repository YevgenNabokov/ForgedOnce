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

namespace AddDisplayNameAttrbutePlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Metadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<Settings, Metadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<Metadata> pluginPreprocessor = null)
        {
            return new Plugin()
            {
                Preprocessor = pluginPreprocessor ?? new AllPropsPreprocessor()
            };
        }
    }
}
