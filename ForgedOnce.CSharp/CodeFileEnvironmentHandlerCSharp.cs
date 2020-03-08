using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Environment.Workspace.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ForgedOnce.CSharp
{
    public class CodeFileEnvironmentHandlerCSharp : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerCSharp(IWorkspaceManager workspaceManager, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
            : base(workspaceManager, new CodeFileCompilationHandlerCSharp(workspaceManager, pipelineExecutionInfo, logger))
        {
        }

        public CodeFileEnvironmentHandlerCSharp(ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {
        }

        protected override CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileCSharp(id, name);
        }
    }
}
