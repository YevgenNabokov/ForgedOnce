using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileStorageHandler
    {
        void Add(CodeFile codeFile);

        void Remove(CodeFile codeFile);

        bool CanResolveCodeFileName(CodeFileLocation location);

        string ResolveCodeFileName(CodeFileLocation location);

        void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true);
    }
}
