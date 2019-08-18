using Game08.Sdk.CodeMixer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ICodeFileCompilationHandler
    {
        bool SupportsCodeLanguage(string language);

        void Register(CodeFile codeFile);

        void Unregister(CodeFile codeFile);

        void RefreshAndRecompile();
    }
}
