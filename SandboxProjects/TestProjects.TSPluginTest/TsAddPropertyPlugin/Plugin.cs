using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstBuilder;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TypeScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TsAddPropertyPlugin
{
    public class Plugin : CodeGenerationFromTsPlugin<Settings, Parameters>
    {
        public const string OutStreamName = "PassThrough";

        public Plugin()
        {
            this.Signature = new ForgedOnce.Core.Plugins.PluginSignature()
            {
                Id = new Guid().ToString(),
                InputLanguage = Languages.TypeScript,
                Name = nameof(Plugin)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.TypeScript, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileTs input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileTs;
            TsSyntaxUtils.CloneContent(input, outFile, metadataRecorder);

            foreach (var c in outFile.Model.statements.OfType<StClassDeclaration>())
            {
                c.WithMember(
                    new StPropertyDeclaration()
                    .WithModifier(new StPublicKeywordToken())
                    .WithName(new StIdentifier().WithEscapedText("f"))
                    .WithType(new StKeywordTypeNodeNumberKeyword()));
            }
        }
    }
}
