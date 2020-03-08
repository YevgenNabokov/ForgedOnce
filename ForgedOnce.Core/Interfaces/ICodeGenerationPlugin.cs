using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeGenerationPlugin
    {
        PluginSignature Signature { get; }

        List<ICodeStream> InitializeOutputs(ICodeStreamFactory codeStreamFactory);

        void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader, ILogger logger);
    }
}
