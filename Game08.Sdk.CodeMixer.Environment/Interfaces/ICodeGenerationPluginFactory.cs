using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ICodeGenerationPluginFactory<TSettings, TMetadata>
    {
        CodeGenerationPlugin<TSettings, TMetadata> CreatePlugin(JObject configuration, IPluginPreprocessor<TMetadata> pluginPreprocessor = null);
    }
}
