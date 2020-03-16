using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Interfaces
{
    public interface ICodeFileLocationSelector<out T> where T : CodeFileLocation
    {
        IEnumerable<T> GetLocations();
    }
}
