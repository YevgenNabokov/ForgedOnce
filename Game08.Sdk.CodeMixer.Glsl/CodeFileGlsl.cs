using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Glsl.Metadata;
using Game08.Sdk.GlslLanguageServices.Builder;
using Game08.Sdk.GlslLanguageServices.Builder.AstAnalysis;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
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
            this.NodePathService = new GlslNodePathService(this);
        }

        public ShaderFile ShaderFile
        {
            get => shaderFile;
            set { this.EnsureFileIsEditable(); shaderFile = value; }
        }

        public INodePathService<AstNode> NodePathService
        {
            get;
            private set;
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
