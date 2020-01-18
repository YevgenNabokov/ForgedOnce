using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IPluginPreprocessor<TCodeFile, TInputParameters, TSettings> where TCodeFile : CodeFile
    {
        TInputParameters GenerateMetadata(TCodeFile input, TSettings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<TCodeFile, GroupItemDetails> fileGroup = null);
    }
}
