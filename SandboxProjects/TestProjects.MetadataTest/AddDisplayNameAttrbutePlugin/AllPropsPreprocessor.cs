using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class AllPropsPreprocessor : IPluginPreprocessor<Metadata>
    {
        public Metadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new Metadata()
            {
                PropertiesToDecorate = (input as CodeFileCSharp).SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
            };
        }
    }
}
