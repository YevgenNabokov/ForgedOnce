using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class AddPropertyPluginFactory : ICodeGenerationPluginFactory<AddPropertySettings, AddPropertyMetadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<AddPropertySettings, AddPropertyMetadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<AddPropertyMetadata> pluginPreprocessor = null)
        {
            return new AddPropertyPlugin()
            {
                Preprocessor = new AddPropertyPreprocessor()
            };
        }
    }
}
