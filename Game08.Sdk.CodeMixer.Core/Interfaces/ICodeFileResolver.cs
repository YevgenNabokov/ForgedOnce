using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeFileResolver
    {
        CodeFile ResolveCodeFile(string language, CodeFileLocation location);
    }
}
