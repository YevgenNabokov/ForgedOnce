using Game08.Sdk.CodeMixer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileLtsText : CodeFile
    {
        public override string Language => Languages.LimitedTypeScript;

        public CodeFileLtsText(string id, string name)
            : base(id, name)
        {
        }

        public string Text;

        protected override string GetSourceCodeText()
        {
            return this.Text;
        }

        protected override void SourceCodeTextUpdated(string newSourceCode)
        {
            this.Text = newSourceCode;
        }
    }
}
