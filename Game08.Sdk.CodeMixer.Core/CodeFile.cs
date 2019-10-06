﻿using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
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

        public int LastRefreshBatchIndex { get; private set; }

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

        public void SetLastRefreshBatchIndex(int index)
        {
            if (this.LastRefreshBatchIndex > index)
            {
                throw new InvalidOperationException($"Cannot set {nameof(LastRefreshBatchIndex)} to older value.");
            }

            this.LastRefreshBatchIndex = index;
        }

        public abstract ISemanticInfoResolver SemanticInfoResolver { get; }

        public abstract IEnumerable<ISemanticSymbol> SemanticSymbols { get; }

        protected abstract string GetSourceCodeText();

        protected abstract void SourceCodeTextUpdated(string newSourceCode);
    }
}
