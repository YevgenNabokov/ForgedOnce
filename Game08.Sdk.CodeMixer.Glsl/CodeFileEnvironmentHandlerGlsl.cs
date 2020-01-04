using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.GlslLanguageServices.Builder;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileEnvironmentHandlerGlsl : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerGlsl(IPipelineExecutionInfo pipelineExecutionInfo, IFileSystem fileSystem, ILogger logger)
            : this(new CodeFileStorageHandlerGlsl(fileSystem), new CodeFileCompilationHandlerGlsl(pipelineExecutionInfo, logger))
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
            var result = new CodeFileGlsl(id, name);
            result.ShaderFile = ShaderFile.CreateEmpty();
            return result;
        }
    }
}
