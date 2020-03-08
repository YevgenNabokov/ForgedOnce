using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Storage;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class AddedPropsPreprocessor : IPluginPreprocessor<Metadata>
    {
        public Metadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            var cSharpFile = input as CodeFileCSharp;

            var propertyAdderActivity = new ActivityFrame(null, "70816D13-0C40-4092-9A28-498FE7A030D0");

            return new Metadata()
            {
                PropertiesToDecorate = cSharpFile
                .SyntaxTree
                .GetRoot()
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(d => metadataReader.SymbolIsGeneratedBy(cSharpFile.SemanticInfoProvider.GetSymbolFor(d), propertyAdderActivity))
            };
        }
    }
}
