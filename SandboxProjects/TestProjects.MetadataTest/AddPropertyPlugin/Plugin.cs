using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Metadata>
    {
        public const string PluginId = "70816D13-0C40-4092-9A28-498FE7A030D0";

        public const string OutStreamName = "PassThrough";

        public Plugin()
        {
            this.Signature = new Game08.Sdk.CodeMixer.Core.Plugins.PluginSignature()
            {
                Id = PluginId,
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

        protected override void Implementation(CodeFileCSharp input, Metadata inputParameters, IMetadataRecorder metadataRecorder)
        {
            PropertyAdder adder = new PropertyAdder();
            var newRoot = adder.Visit(input.SyntaxTree.GetRoot());

            foreach (var added in adder.AddedProperties)
            {
                metadataRecorder.SymbolGenerated<SyntaxNode>(input.SemanticInfoProvider, added, new HashSet<string>());
            }

            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileCSharp;
            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);
        }
    }
}
