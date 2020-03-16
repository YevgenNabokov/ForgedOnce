using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Pipeline;
using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IPipelineEnvironment : ICodeFileResolver
    {
        IPipelineExecutionInfo PipelineExecutionInfo { get; }

        IEnumerable<CodeFile> GetOutputs();

        void CodeStreamsCompleted(IEnumerable<ICodeStream> streams);

        void StoreForOutput(IEnumerable<CodeFile> files);

        void CodeStreamsDiscarded(IEnumerable<ICodeStream> streams);

        void RefreshAndRecompile();

        ICodeStream CreateCodeStream(string language, string name, ICodeFileDestination codeFileLocationProvider = null);

        ShadowFilter GetShadowFilter(string language);
    }
}
