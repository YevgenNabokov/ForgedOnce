using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Tests.TestClasses
{
    public class TestPluginFactory : ICodeGenerationPluginFactory<TestPluginSettings, TestPluginMetadata, CodeFileCSharp>
    {
        public ICodeGenerationPlugin CreatePlugin(JObject configuration, IPluginPreprocessor<CodeFileCSharp, TestPluginMetadata, TestPluginSettings> pluginPreprocessor = null)
        {
            return new TestPlugin();
        }
    }
}
