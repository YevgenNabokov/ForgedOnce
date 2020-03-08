using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Metadata>
    {
        public const string OutStreamName = "PassThrough";

        public Plugin()
        {
            this.Signature = new ForgedOnce.Core.Plugins.PluginSignature()
            {
                Id = new Guid().ToString(),
                InputLanguage = Languages.CSharp,
                Name = nameof(Plugin)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.CSharp, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, Metadata inputParameters, IMetadataRecorder metadataRecorder)
        {
            AttributeAdder serializableAttributeAdder = new AttributeAdder(inputParameters.PropertiesToDecorate);
            var newRoot = serializableAttributeAdder.Visit(input.SyntaxTree.GetRoot());

            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileCSharp;
            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);
        }
    }
}
