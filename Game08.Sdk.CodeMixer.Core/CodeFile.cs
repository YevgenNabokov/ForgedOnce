using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public abstract class CodeFile
    {
        public string Id;

        public string Name;

        public abstract string Language { get; }
    }
}
