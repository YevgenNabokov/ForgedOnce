using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.TsLanguageServices.Host;
using ForgedOnce.TsLanguageServices.Host.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public class CodeFileEnvironmentHandlerTs : CodeFileEnvironmentHandler
    {
        private readonly IFileSystem fileSystem;

        private readonly ITsHost tsHost;

        public CodeFileEnvironmentHandlerTs(IFileSystem fileSystem, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
            : this(fileSystem, new CodeFileStorageHandlerTs(), new CodeFileCompilationHandlerTs(pipelineExecutionInfo, logger), pipelineExecutionInfo)
        {            
        }

        public CodeFileEnvironmentHandlerTs(
            IFileSystem fileSystem,
            ICodeFileStorageHandler codeFileStorageHandler,
            ICodeFileCompilationHandler codeFileCompilationHandler,
            IPipelineExecutionInfo pipelineExecutionInfo)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {
            this.fileSystem = fileSystem;
            this.tsHost = new TsHost(30050, 30100, 5000);
        }

        protected override Core.CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileTs(id, name, this.tsHost);
        }
    }
}
