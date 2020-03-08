using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeStreamFilter
    {
        IEnumerable<ICodeStream> Filter(IEnumerable<ICodeStream> streams);
    }
}
