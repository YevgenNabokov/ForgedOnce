using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeStream
    {
        List<CodeFile> Files { get; }

        string Language { get; }

        string Name { get; }
    }
}
