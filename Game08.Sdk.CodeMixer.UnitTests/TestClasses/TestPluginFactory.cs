using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests.TestClasses
{
    public class TestPluginFactory : ICodeGenerationPluginFactory<TestPluginSettings, TestPluginMetadata, CodeFileCSharp>
    {
        public CodeGenerationPlugin<TestPluginSettings, TestPluginMetadata, CodeFileCSharp> CreatePlugin(JObject configuration, IPluginPreprocessor<CodeFileCSharp, TestPluginMetadata> pluginPreprocessor = null)
        {
            return new TestPlugin();
        }
    }
}
