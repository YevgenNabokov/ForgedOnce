using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Pipeline;
using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment
{
    public abstract class CodeFileEnvironmentHandler : ICodeFileEnvironmentHandler
    {
        protected readonly ICodeFileStorageHandler storageHandler;
        protected readonly ICodeFileCompilationHandler compilationHandler;

        protected List<CodeFile> output = new List<CodeFile>();

        public CodeFileEnvironmentHandler(ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
        {
            this.ShadowFilter = new ShadowFilter();
            this.storageHandler = codeFileStorageHandler;
            this.compilationHandler = codeFileCompilationHandler;
            this.compilationHandler.ShadowFilter = this.ShadowFilter;
        }

        public ShadowFilter ShadowFilter { get; protected set; }

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

        public virtual bool CanResolveExistingCodeFile(CodeFileLocation location)
        {
            return this.storageHandler.CanResolveCodeFileName(location);
        }

        public virtual CodeFile ResolveExistingCodeFile(CodeFileLocation location)
        {
            if (!this.storageHandler.CanResolveCodeFileName(location))
            {
                throw new InvalidOperationException($"Unable to resolve code file at location ({location})");
            }

            var codeFile = this.CreateCodeFile(this.storageHandler.ResolveCodeFileName(location));
            codeFile.Location = location;
            this.storageHandler.ResolveCodeFile(codeFile);
            this.compilationHandler.Register(codeFile);
            return codeFile;
        }

        public virtual ICodeStream CreateCodeStream(string language, string name, ICodeFileDestination codeFileLocationProvider = null)
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
