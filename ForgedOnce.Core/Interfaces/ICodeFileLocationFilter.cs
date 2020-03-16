using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Interfaces
{
    public interface ICodeFileLocationFilter
    {
        bool IsMatch(CodeFileLocation location);
    }
}
