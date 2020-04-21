using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.TypeScript;
using System;
using System.Collections.Generic;
using System.Text;

namespace TsAddPropertyPlugin
{
    public class Preprocessor : IPluginPreprocessor<CodeFileTsModel, Parameters, Settings>
    {
        public Parameters GenerateParameters(CodeFileTsModel input, Settings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<CodeFileTsModel, GroupItemDetails> fileGroup = null)
        {
            return new Parameters();
        }
    }
}
