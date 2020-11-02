using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstBuilder;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TypeScript;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TsTestPlugin
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
            result.Add(codeStreamFactory.CreateCodeStream(Languages.TypeScript, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            var outFile = this.Outputs[OutStreamName].CreateCodeFile($"{Path.GetFileNameWithoutExtension(input.Name)}.ts") as CodeFileTs;
            outFile.Model = new StRoot();


            foreach (var name in inputParameters.ClassNames)
            {
                var definition = new StClassDeclaration()
                    .WithName(new StIdentifier().WithEscapedText(name))
                    .WithModifier(new StExportKeywordToken());

                outFile.Model.statements.Add(definition);
            }
        }
    }
}
