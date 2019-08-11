using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeGenerationPlugin
    {
        List<ICodeStream> InitializeOutputs();

        void Execute(IEnumerable<CodeFile> input);
    }
}
