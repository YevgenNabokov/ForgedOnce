using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjects.SimplePlugin
{
    public class AddSerializableAttributeToClassPluginFactory : ICodeGenerationPluginFactory<AddSerializableAttributeToClassSettings, AddSerializableAttributeToClassMetadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<AddSerializableAttributeToClassSettings, AddSerializableAttributeToClassMetadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<AddSerializableAttributeToClassMetadata> pluginPreprocessor = null)
        {
            return new AddSerializableAttributeToClassPlugin();
        }
    }
}
