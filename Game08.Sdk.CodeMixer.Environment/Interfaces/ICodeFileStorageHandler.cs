using Game08.Sdk.CodeMixer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ICodeFileStorageHandler
    {
        void Add(CodeFile codeFile);

        void Remove(CodeFile codeFile);

        void ResolveSourceCodeText(CodeFile codeFile);
    }
}
