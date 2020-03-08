using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeStream
    {
        IEnumerable<CodeFile> Files { get; }

        string Language { get; }

        string Name { get; }

        bool IsReadonly { get; }

        CodeFile CreateCodeFile(string name);
    }
}
