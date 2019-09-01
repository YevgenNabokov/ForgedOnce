using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileStorageHandlerLts : ICodeFileStorageHandler
    {
        private List<CodeFile> codeFiles = new List<CodeFile>();

        public void Add(CodeFile codeFile)
        {
            if (!this.codeFiles.Contains(codeFile))
            {
                this.codeFiles.Add(codeFile);
            }
        }

        public void Remove(CodeFile codeFile)
        {
            if (this.codeFiles.Contains(codeFile))
            {
                this.codeFiles.Remove(codeFile);
            }
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            throw new NotSupportedException();
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            throw new NotSupportedException();
        }
    }
}
