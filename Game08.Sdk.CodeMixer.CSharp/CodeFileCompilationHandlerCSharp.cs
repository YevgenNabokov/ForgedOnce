using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileCompilationHandlerCSharp : ICodeFileCompilationHandler
    {
        private IWorkspaceManager workspaceManager;

        private List<CodeFileCSharp> codeFiles = new List<CodeFileCSharp>();

        public CodeFileCompilationHandlerCSharp(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public void RefreshAndRecompile()
        {
            throw new NotImplementedException();
        }

        public void Register(CodeFile codeFile)
        {
            if (codeFile != null)
            {
                if (!(codeFile is CodeFileCSharp))
                {
                    throw new NotSupportedException($"{nameof(CodeFileCompilationHandlerCSharp)} supports only {typeof(CodeFileCSharp)}.");
                }

                this.codeFiles.Add(codeFile as CodeFileCSharp);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return language == Languages.CSharp;
        }

        public void Unregister(CodeFile codeFile)
        {
            if (codeFile != null)
            {
                if (!(codeFile is CodeFileCSharp))
                {
                    throw new NotSupportedException($"{nameof(CodeFileCompilationHandlerCSharp)} supports only {typeof(CodeFileCSharp)}.");
                }

                this.codeFiles.Remove(codeFile as CodeFileCSharp);
            }
        }
    }
}
