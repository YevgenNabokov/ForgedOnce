using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileEnvironmentHandlerLts : CodeFileEnvironmentHandler
    {
        private readonly IFileSystem fileSystem;

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem)
            : this(fileSystem, new CodeFileStorageHandlerLts(), new CodeFileCompilationHandlerLts())
        {
        }

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem, ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {
            this.fileSystem = fileSystem;
        }

        public override IEnumerable<CodeFile> GetOutputs()
        {
            var models = base.GetOutputs().Cast<CodeFileLtsModel>();

            throw new NotImplementedException();
        }

        protected override CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileLtsModel(id, name);
        }
    }
}
