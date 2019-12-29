using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.CSharp.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileCSharp : CodeFile
    {
        private SyntaxTree syntaxTree;
        private SemanticModel semanticModel;

        public override string Language => Languages.CSharp;

        public CodeFileCSharp(string id, string name)
            : base(id, name)
        {
            this.NodePathService = new CSharpNodePathService(this);
        }

        public SyntaxTree SyntaxTree
        {
            get => this.syntaxTree;
            set { this.EnsureFileIsEditable(); this.syntaxTree = value; }
        }

        public SemanticModel SemanticModel
        {
            get => this.semanticModel;
            set { this.EnsureFileIsEditable(); this.semanticModel = value; }
        }

        public INodePathService<SyntaxNode> NodePathService
        {
            get;
            private set;
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
