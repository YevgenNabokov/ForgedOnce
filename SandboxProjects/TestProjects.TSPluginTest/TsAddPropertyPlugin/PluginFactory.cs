using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.TypeScript;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TsAddPropertyPlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Parameters, CodeFileTsModel>
    {
        public ICodeGenerationPlugin CreatePlugin(JObject configuration, IPluginPreprocessor<CodeFileTsModel, Parameters, Settings> pluginPreprocessor = null)
        {
            return new Plugin()
            {
                Preprocessor = new Preprocessor()
            };
        }
    }
}
