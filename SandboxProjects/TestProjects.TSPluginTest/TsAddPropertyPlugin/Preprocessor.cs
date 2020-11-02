using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.TypeScript;
using System;
using System.Collections.Generic;
using System.Text;

namespace TsAddPropertyPlugin
{
    public class Preprocessor : IPluginPreprocessor<CodeFileTs, Parameters, Settings>
    {
        public Parameters GenerateParameters(CodeFileTs input, Settings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<CodeFileTs, GroupItemDetails> fileGroup = null)
        {
            return new Parameters();
        }
    }
}
