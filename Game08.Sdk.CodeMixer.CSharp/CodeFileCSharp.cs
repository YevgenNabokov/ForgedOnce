using Game08.Sdk.CodeMixer.Core;
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
        }

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
