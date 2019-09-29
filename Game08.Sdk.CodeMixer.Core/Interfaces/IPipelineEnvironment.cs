using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IPipelineEnvironment : ICodeFileResolver
    {
        IPipelineExecutionInfo PipelineExecutionInfo { get; }

        IEnumerable<CodeFile> GetOutputs();

        void CodeStreamsCompleted(IEnumerable<ICodeStream> streams);

        void StoreForOutput(IEnumerable<CodeFile> files);

        void CodeStreamsDiscarded(IEnumerable<ICodeStream> streams);

        void RefreshAndRecompile();

        ICodeStream CreateCodeStream(string language, string name, ICodeFileLocationProvider codeFileLocationProvider = null);
    }
}
