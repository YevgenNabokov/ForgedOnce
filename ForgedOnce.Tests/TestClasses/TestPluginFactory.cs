using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Tests.TestClasses
{
    public class TestPluginFactory : ICodeGenerationPluginFactory<TestPluginSettings, TestPluginMetadata, CodeFileCSharp>
    {
        public ICodeGenerationPlugin CreatePlugin(JObject configuration, IPipelineCreationContext context, IPluginPreprocessor<CodeFileCSharp, TestPluginMetadata, TestPluginSettings> pluginPreprocessor = null)
        {
            return new TestPlugin();
        }
    }
}
