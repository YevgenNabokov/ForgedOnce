using ForgedOnce.Core;
using ForgedOnce.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileCompilationHandler
    {
        ShadowFilter ShadowFilter { get; set; }

        bool SupportsCodeLanguage(string language);

        void Register(CodeFile codeFile);

        void Unregister(CodeFile codeFile);

        void RefreshAndRecompile();
    }
}
