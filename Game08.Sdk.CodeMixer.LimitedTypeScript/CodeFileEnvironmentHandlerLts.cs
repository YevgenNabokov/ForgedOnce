using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.LTS.Builder.TypeData;
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

        private LtsTypeRepository typeRepository;

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem)
            : this(fileSystem, new CodeFileStorageHandlerLts(), new CodeFileCompilationHandlerLts())
        {            
        }

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem, ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {
            this.fileSystem = fileSystem;
            this.typeRepository = new LtsTypeRepository();
        }

        public override IEnumerable<CodeFile> GetOutputs()
        {
            var models = base.GetOutputs().Cast<CodeFileLtsModel>();



            throw new NotImplementedException();
        }

        protected override CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileLtsModel(id, name, this.typeRepository);
        }
    }
}
