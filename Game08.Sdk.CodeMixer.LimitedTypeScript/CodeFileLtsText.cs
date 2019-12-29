using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileLtsText : CodeFile
    {
        private string text;

        public override string Language => Languages.LimitedTypeScript;

        public CodeFileLtsText(string id, string name)
            : base(id, name)
        {
        }

        public string Text
        {
            get => text;
            set { this.EnsureFileIsEditable(); text = value; }
        }

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
