using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileResolver
    {
        CodeFile ResolveCodeFile(string language, CodeFileLocation location);
    }
}
