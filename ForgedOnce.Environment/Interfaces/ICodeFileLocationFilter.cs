using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileLocationFilter<T> where T: CodeFileLocation
    {
        bool IsMatch(T location);
    }
}
