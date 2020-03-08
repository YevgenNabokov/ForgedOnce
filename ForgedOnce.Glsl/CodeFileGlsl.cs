using ForgedOnce.Core;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Glsl.Metadata;
using ForgedOnce.GlslLanguageServices.Builder;
using ForgedOnce.GlslLanguageServices.Builder.AstAnalysis;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Glsl
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

        public override void MakeReadOnly()
        {
            base.MakeReadOnly();
            this.ShaderFile?.SyntaxTree?.MakeReadonly();
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
