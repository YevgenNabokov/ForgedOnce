using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public abstract class CodeFile
    {
        private string sourceCodeText;

        public CodeFile(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public abstract string Language { get; }

        public CodeFileLocation Location { get; set; }

        public string SourceCodeText
        {
            get
            {
                var text = this.GetSourceCodeText();
                if (text != null)
                {
                    this.sourceCodeText = text;
                }

                return sourceCodeText;
            }

            set
            {
                this.sourceCodeText = value;
                this.SourceCodeTextUpdated(value);
            }
        }

        protected abstract string GetSourceCodeText();

        protected abstract void SourceCodeTextUpdated(string newSourceCode);
    }
}
