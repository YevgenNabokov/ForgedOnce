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

        private List<CodeFile> output = new List<CodeFile>();

        public CodeFileEnvironmentHandler(ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
        {
            this.storageHandler = codeFileStorageHandler;
            this.compilationHandler = codeFileCompilationHandler;
        }

        public virtual IEnumerable<CodeFile> GetOutputs()
        {
            return this.output;
        }

        public virtual void AddOutput(CodeFile codeFile)
        {
            if (!this.output.Contains(codeFile))
            {
                this.output.Add(codeFile);
            }
        }

        public virtual void Add(CodeFile codeFile)
        {
            this.storageHandler.Add(codeFile);
            this.compilationHandler.Register(codeFile);
        }

        public virtual void RefreshAndRecompile()
        {
            this.compilationHandler.RefreshAndRecompile();
        }

        public virtual void Remove(CodeFile codeFile)
        {
            if (!this.output.Contains(codeFile))
            {
                this.compilationHandler.Unregister(codeFile);
                this.storageHandler.Remove(codeFile);
            }
        }

        public virtual bool SupportsCodeLanguage(string language)
        {
            return this.compilationHandler.SupportsCodeLanguage(language);
        }

        public virtual CodeFile ResolveExistingCodeFile(CodeFileLocation location)
        {
            var codeFile = this.CreateCodeFile(this.storageHandler.ResolveCodeFileName(location));
            codeFile.Location = location;
            this.storageHandler.ResolveCodeFile(codeFile);
            this.compilationHandler.Register(codeFile);
            return codeFile;
        }

        public virtual ICodeStream CreateCodeStream(string language, string name, ICodeFileLocationProvider codeFileLocationProvider = null)
        {
            return new CodeStream(language, name, this, codeFileLocationProvider);
        }

        public CodeFile CreateCodeFile(string name)
        {
            return this.CreateCodeFile(Guid.NewGuid().ToString(), name);
        }

        protected abstract CodeFile CreateCodeFile(string id, string name);
    }
}
