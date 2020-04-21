using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.Model.DefinitionTree;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using ForgedOnce.TsLanguageServices.ModelBuilder.ExtensionMethods;
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
                InputLanguage = Languages.LimitedTypeScript,
                Name = nameof(Plugin)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.LimitedTypeScript, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileTsModel input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileTsModel;
            TsSyntaxUtils.CloneContent(input, outFile, metadataRecorder);

            foreach (var c in outFile.Model.Items.OfType<ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree.ClassDefinition>())
            {
                c.Fields.Add(
                    new ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree.FieldDeclaration()
                    .WithModifiers(Modifier.Public)
                    .WithName("f")
                    .WithTypeReference(outFile.TypeRepository.RegisterTypeReferenceBuiltin("number")));
            }
        }
    }
}
