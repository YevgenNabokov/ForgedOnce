using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public class CodeStream : ICodeStream
    {
        protected List<CodeFile> codeFiles = new List<CodeFile>();

        public CodeStream(string language, string name)
        {
            this.Language = language;
            this.Name = name;
        }

        public IEnumerable<CodeFile> Files
        {
            get
            {
                return this.codeFiles;
            }
        }

        public string Language { get; protected set; }

        public string Name { get; protected set; }

        public virtual void AddCodeFile(CodeFile file)
        {
            this.codeFiles.Add(file);
        }
    }
}
