using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeStreamFactory
    {
        ICodeStream CreateCodeStream(string language, string name);
    }
}
