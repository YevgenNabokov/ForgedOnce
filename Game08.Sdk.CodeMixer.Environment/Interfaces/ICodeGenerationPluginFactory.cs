﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ICodeGenerationPluginFactory<TSettings, TMetadata, TCodeFile> where TCodeFile : CodeFile
    {
        ICodeGenerationPlugin CreatePlugin(JObject configuration, IPluginPreprocessor<TCodeFile, TMetadata> pluginPreprocessor = null);
    }
}
