using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileEnvironmentHandlerCSharp : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerCSharp(IWorkspaceManager workspaceManager, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
            : base(new WorkspaceFileStorageHandler(workspaceManager), new CodeFileCompilationHandlerCSharp(workspaceManager, pipelineExecutionInfo, logger))
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
