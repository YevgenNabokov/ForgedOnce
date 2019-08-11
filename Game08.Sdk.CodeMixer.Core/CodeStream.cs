using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public class CodeStream : ICodeStream
    {
        public List<CodeFile> Files { get; protected set; }

        public string Language { get; protected set; }

        public string Name { get; protected set; }
    }
}
