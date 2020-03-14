using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileEnvironmentHandler : ICodeFileFactory
    {
        bool SupportsCodeLanguage(string language);

        IEnumerable<CodeFile> GetOutputs();

        void AddOutput(CodeFile codeFile);

        void Add(CodeFile codeFile);

        void Remove(CodeFile codeFile);

        void RefreshAndRecompile();

        bool CanResolveExistingCodeFile(CodeFileLocation location);

        CodeFile ResolveExistingCodeFile(CodeFileLocation location);

        ICodeStream CreateCodeStream(string language, string name, ICodeFileDestination codeFileLocationProvider = null);
    }
}
