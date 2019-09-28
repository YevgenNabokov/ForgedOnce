using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileCSharp : CodeFile
    {
        public override string Language => Languages.CSharp;        

        public SyntaxTree SyntaxTree;

        public SemanticModel SemanticModel;

        public CodeFileCSharp(string id, string name)
            :base(id, name)
        {
            this.SemanticInfoProvider = new SemanticInfoProvider(this);
        }

        public SemanticInfoProvider SemanticInfoProvider
        {
            get;
            private set;
        }

        public override ISemanticInfoResolver SemanticInfoResolver => this.SemanticInfoProvider;

        protected override string GetSourceCodeText()
        {
            if (this.SyntaxTree != null)
            {
                return this.SyntaxTree.GetRoot().NormalizeWhitespace().ToFullString();
            }

            return null;
        }

        protected override void SourceCodeTextUpdated(string sourceCodeText)
        {
            this.SyntaxTree = null;
            this.SemanticModel = null;
        }
    }
}
