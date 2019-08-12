using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IPipelineEnvironment
    {
        void CodeStreamsEmitted(IEnumerable<ICodeStream> streams);

        void StoreForOutput(IEnumerable<CodeFile> files);

        void CodeStreamsDiscarded(IEnumerable<ICodeStream> streams);
    }
}
