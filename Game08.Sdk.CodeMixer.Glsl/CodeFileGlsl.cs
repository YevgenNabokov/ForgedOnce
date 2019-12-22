using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Glsl.Metadata;
using Game08.Sdk.GlslLanguageServices.Builder;
using Game08.Sdk.GlslLanguageServices.Builder.AstAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileGlsl : CodeFile
    {
        private ShaderFile shaderFile;

        public override string Language => Languages.Glsl;

        public CodeFileGlsl(string id, string name)
            : base(id, name)
        {
            this.SemanticInfoProvider = new SemanticInfoProvider(this);
        }

        public ShaderFile ShaderFile
        {
            get => shaderFile;
            set { this.EnsureFileIsEditable(); shaderFile = value; }
        }

        public SemanticInfoProvider SemanticInfoProvider
        {
            get;
            private set;
        }

        public override ISemanticInfoResolver SemanticInfoResolver => this.SemanticInfoProvider;

        public override IEnumerable<ISemanticSymbol> SemanticSymbols
        {
            get
            {
                if (this.ShaderFile != null)
                {
                    var visitor = new SearchVisitor();
                    foreach (var node in visitor.Select(this.ShaderFile.SyntaxTree, null, null).Where(n => this.SemanticInfoProvider.CanGetSymbolFor(n)))
                    {
                        yield return this.SemanticInfoProvider.GetSymbolFor(node);
                    }
                }
            }
        }

        protected override string GetSourceCodeText()
        {
            return this.ShaderFile.ToText();
        }

        protected override void SourceCodeTextUpdated(string sourceCodeText)
        {
            this.ShaderFile = ShaderFile.CreateFromText(sourceCodeText);
        }
    }
}
