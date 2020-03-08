using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileSelector
    {
        IEnumerable<CodeFile> Select(IEnumerable<ICodeStream> streams);
    }
}
