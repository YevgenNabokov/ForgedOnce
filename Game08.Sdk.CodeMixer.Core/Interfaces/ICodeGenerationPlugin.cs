using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeGenerationPlugin
    {
        PluginSignature Signature { get; }

        List<ICodeStream> InitializeOutputs(ICodeStreamFactory codeStreamFactory);

        void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader, ILogger logger);
    }
}
