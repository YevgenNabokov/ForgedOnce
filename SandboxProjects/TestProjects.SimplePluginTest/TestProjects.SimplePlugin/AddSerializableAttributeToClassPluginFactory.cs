using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
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
            return new AddSerializableAttributeToClassPlugin()
            {
                Preprocessor = new AddSerializableAttributeToClassPreprocessor()
            };
        }
    }
}
