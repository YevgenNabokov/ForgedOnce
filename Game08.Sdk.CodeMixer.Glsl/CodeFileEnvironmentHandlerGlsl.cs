using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileEnvironmentHandlerGlsl : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerGlsl()
            : this(new CodeFileStorageHandlerGlsl(), new CodeFileCompilationHandlerGlsl())
        {
        }

        public CodeFileEnvironmentHandlerGlsl(
            ICodeFileStorageHandler codeFileStorageHandler,
            ICodeFileCompilationHandler codeFileCompilationHandler)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {                   
        }

        protected override CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileGlsl(id, name);
        }
    }
}
