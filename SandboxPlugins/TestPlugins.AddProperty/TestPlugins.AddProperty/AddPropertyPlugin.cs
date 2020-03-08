using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class AddPropertyPlugin : CodeGenerationFromCSharpPlugin<AddPropertySettings, AddPropertyMetadata>
    {
        public const string OutStreamName = "PassThrough";

        public AddPropertyPlugin()
        {
            this.Signature = new ForgedOnce.Core.Plugins.PluginSignature()
            {
                Id = new Guid().ToString(),
                InputLanguage = Languages.CSharp,
                Name = nameof(AddPropertyPlugin)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.CSharp, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, AddPropertyMetadata inputParameters, IMetadataRecorder metadataRecorder)
        {
            PropertyAdder serializableAttributeAdder = new PropertyAdder();
            var newRoot = serializableAttributeAdder.Visit(input.SyntaxTree.GetRoot());

            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileCSharp;
            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);
        }
    }
}
