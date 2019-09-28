using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class SemanticInfoProvider : ISemanticInfoProvider<AstNode>
    {
        private readonly CodeFileGlsl codeFileGlsl;

        public SemanticInfoProvider(CodeFileGlsl codeFileGlsl)
        {
            this.codeFileGlsl = codeFileGlsl;
        }

        public ISemanticSymbol GetSymbol(AstNode astNode)
        {
            throw new NotImplementedException();
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
