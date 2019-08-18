using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment
{
    public abstract class CodeFileEnvironmentHandler : ICodeFileEnvironmentHandler
    {
        private readonly ICodeFileStorageHandler storageHandler;
        private readonly ICodeFileCompilationHandler compilationHandler;

        public CodeFileEnvironmentHandler(ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
        {
            this.storageHandler = codeFileStorageHandler;
            this.compilationHandler = codeFileCompilationHandler;
        }

        public void Add(CodeFile codeFile)
        {
            this.storageHandler.Add(codeFile);
            this.compilationHandler.Register(codeFile);
        }

        public void RefreshAndRecompile()
        {
            this.compilationHandler.RefreshAndRecompile();
        }

        public void Remove(CodeFile codeFile)
        {
            this.compilationHandler.Unregister(codeFile);
            this.storageHandler.Remove(codeFile);
        }

        public bool SupportsCodeLanguage(string language)
        {
            return this.compilationHandler.SupportsCodeLanguage(language);
        }

        public CodeFile ResolveExistingCodeFile(CodeFileLocation location)
        {
            var codeFile = this.CreateCodeFile(Guid.NewGuid().ToString(), location.GetFileName());
            this.storageHandler.ResolveSourceCodeText(codeFile);
            this.compilationHandler.Register(codeFile);
            return codeFile;
        }

        protected abstract CodeFile CreateCodeFile(string id, string name);
    }
}
