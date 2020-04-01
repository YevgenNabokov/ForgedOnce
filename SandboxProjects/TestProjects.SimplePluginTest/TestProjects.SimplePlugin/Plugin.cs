using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjects.SimplePlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Parameters>
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

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            SerializableAttributeAdder serializableAttributeAdder = new SerializableAttributeAdder();
            var newRoot = serializableAttributeAdder.Visit(input.SyntaxTree.GetRoot());

            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileCSharp;
            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);
        }
    }
}
