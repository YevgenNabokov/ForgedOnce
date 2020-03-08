using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileDestination
    {
        CodeFileLocation GetLocation(string fileName);

        void Clear();
    }
}
