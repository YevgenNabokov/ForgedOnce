using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileEnvironmentHandlerCSharp : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerCSharp(IWorkspaceManager workspaceManager, IPipelineExecutionInfo pipelineExecutionInfo)
            : base(new WorkspaceFileStorageHandler(workspaceManager), new CodeFileCompilationHandlerCSharp(workspaceManager, pipelineExecutionInfo))
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
