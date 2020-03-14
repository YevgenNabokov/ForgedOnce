using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileResolver
    {
        bool CanResolveCodeFile(string language, CodeFileLocation location);

        CodeFile ResolveCodeFile(string language, CodeFileLocation location);
    }
}
