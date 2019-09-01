using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeFileEnvironmentHandler : ICodeFileFactory
    {
        bool SupportsCodeLanguage(string language);

        IEnumerable<CodeFile> GetOutputs();

        void AddOutput(CodeFile codeFile);

        void Add(CodeFile codeFile);

        void Remove(CodeFile codeFile);

        void RefreshAndRecompile();

        CodeFile ResolveExistingCodeFile(CodeFileLocation location);

        ICodeStream CreateCodeStream(string language, string name, ICodeFileLocationProvider codeFileLocationProvider = null);
    }
}
