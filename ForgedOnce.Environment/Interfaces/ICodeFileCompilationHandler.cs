using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileCompilationHandler
    {
        bool SupportsCodeLanguage(string language);

        void Register(CodeFile codeFile);

        void Unregister(CodeFile codeFile);

        void RefreshAndRecompile();
    }
}
