using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.GlslLanguageServices.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileEnvironmentHandlerGlsl : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerGlsl(IPipelineExecutionInfo pipelineExecutionInfo)
            : this(new CodeFileStorageHandlerGlsl(), new CodeFileCompilationHandlerGlsl(pipelineExecutionInfo))
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
