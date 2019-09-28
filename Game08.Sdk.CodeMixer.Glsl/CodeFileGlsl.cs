using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileGlsl : CodeFile
    {
        public override string Language => Languages.Glsl;

        public ShaderFile ShaderFile;

        public CodeFileGlsl(string id, string name)
            : base(id, name)
        {
        }

        public SemanticInfoProvider SemanticInfoProvider
        {
            get;
            private set;
        }

        public override ISemanticInfoResolver SemanticInfoResolver => this.SemanticInfoProvider;

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
