using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeStreamProvider
    {
        IEnumerable<ICodeStream> RetrieveCodeStreams();
    }
}
