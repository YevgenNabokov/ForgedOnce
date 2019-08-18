using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileEnvironmentHandlerCSharp : CodeFileEnvironmentHandler
    {
        public CodeFileEnvironmentHandlerCSharp(IWorkspaceManager workspaceManager)
            : base(new WorkspaceFileStorageHandler(workspaceManager), new CodeFileCompilationHandlerCSharp(workspaceManager))
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
