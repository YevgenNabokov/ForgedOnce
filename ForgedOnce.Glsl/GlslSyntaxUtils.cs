using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.GlslLanguageServices.Builder;
using ForgedOnce.GlslLanguageServices.Builder.SyntaxTools;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Glsl
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
                    metadataRecorder.SymbolSourcingFrom(source.NodePathService, declaration, target.NodePathService, targetNode, new Dictionary<string, string>());
                }
            }
        }

        public static int IndexAfterVariablesBeforeFunctions(IEnumerable<Declaration> declarations)
        {
            int i = 0;
            foreach (var declaration in declarations)
            {
                if (declaration is FunctionDeclaration)
                {
                    return i;
                }

                i++;
            }

            return i;
        }
    }
}
