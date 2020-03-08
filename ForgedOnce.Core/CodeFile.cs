using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core
{
    public abstract class CodeFile
    {
        private string sourceCodeText;
        private CodeFileLocation location;

        public CodeFile(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public bool IsReadOnly { get; private set; }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public abstract string Language { get; }

        public CodeFileLocation Location
        {
            get => location;
            set 
            {
                this.EnsureFileIsEditable();
                location = value;
            }
        }

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
                this.EnsureFileIsEditable();
                this.sourceCodeText = value;
                this.SourceCodeTextUpdated(value);
            }
        }

        public virtual void MakeReadOnly()
        {
            this.IsReadOnly = true;
        }

        protected abstract string GetSourceCodeText();

        protected abstract void SourceCodeTextUpdated(string newSourceCode);

        protected void EnsureFileIsEditable()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException($"Attempt to modify readonly {nameof(CodeFile)}.");
            }
        }
    }
}
