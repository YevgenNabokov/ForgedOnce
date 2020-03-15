using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileLocationSelector<out T> where T : CodeFileLocation
    {
        IEnumerable<T> GetLocations();
    }
}
