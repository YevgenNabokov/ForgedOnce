using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IPluginPreprocessor<TCodeFile, TInputParameters, TSettings> where TCodeFile : CodeFile
    {
        TInputParameters GenerateMetadata(TCodeFile input, TSettings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<TCodeFile, GroupItemDetails> fileGroup = null);
    }
}
