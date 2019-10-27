using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.GlslLanguageServices.Builder;
using Game08.Sdk.GlslLanguageServices.Builder.SyntaxTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public static class GlslSyntaxUtils
    {
        public static void CloneContent(CodeFileGlsl source, CodeFileGlsl target, IMetadataRecorder metadataRecorder)
        {
            if (source.ShaderFile != null)
            {
                CloningAstVisitor cloner = new CloningAstVisitor();
                if (target.ShaderFile == null)
                {
                    target.ShaderFile = ShaderFile.CreateEmpty(source.ShaderFile.SyntaxTree.Version);
                }

                target.ShaderFile.SyntaxTree.Version = source.ShaderFile.SyntaxTree.Version;
                foreach (var declaration in source.ShaderFile.SyntaxTree.Declarations)
                {
                    var targetNode = cloner.CloneNode(declaration);
                    target.ShaderFile.SyntaxTree.Declarations.Add(targetNode);
                    metadataRecorder.SymbolSourcingFrom(source.SemanticInfoProvider, declaration, target.SemanticInfoProvider, targetNode, new Dictionary<string, string>());
                }
            }
        }        
    }
}
