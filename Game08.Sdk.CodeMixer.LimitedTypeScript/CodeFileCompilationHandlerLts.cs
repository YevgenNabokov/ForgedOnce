using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileCompilationHandlerLts : ICodeFileCompilationHandler
    {
        private List<CodeFileLtsModel> codeFiles = new List<CodeFileLtsModel>();

        public void RefreshAndRecompile()
        {
            this.RefreshTypeRepository();
        }

        public void Register(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileLtsModel;
            if (!this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Add(ltsCodeFile);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return Languages.LimitedTypeScript == language;
        }

        public void Unregister(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileLtsModel;
            if (this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Remove(ltsCodeFile);
            }
        }

        private void RefreshTypeRepository()
        {
            foreach (var codeFile in this.codeFiles)
            {
                
            }

            throw new NotImplementedException();
        }
    }
}
