using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeGenerationPluginFactory<TSettings, TInputParameters, TCodeFile> where TCodeFile : CodeFile
    {
        ICodeGenerationPlugin CreatePlugin(JObject configuration, IPluginPreprocessor<TCodeFile, TInputParameters, TSettings> pluginPreprocessor = null);
    }
}
