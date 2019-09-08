using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.LimitedTypeScript;
using Game08.Sdk.LTS.Model.DefinitionTree;
using Game08.Sdk.LTS.Model.TypeData;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LtsTestPlugin
{
    public class LtsTestPluginImplementation : CodeGenerationFromCSharpPlugin<LtsTestPluginSettings, LtsTestPluginMetadata>
    {
        public const string OutStreamName = "PassThrough";

        public LtsTestPluginImplementation()
        {
            this.Signature = new Game08.Sdk.CodeMixer.Core.Plugins.PluginSignature()
            {
                Id = new Guid().ToString(),
                InputLanguage = Languages.CSharp,
                Name = nameof(LtsTestPluginImplementation)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.LimitedTypeScript, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, LtsTestPluginMetadata metadata, IMetadataWriter outputMetadataWriter)
        {
            var outFile = this.Outputs[OutStreamName].CreateCodeFile($"{Path.GetFileNameWithoutExtension(input.Name)}.ts") as CodeFileLtsModel;
            outFile.Model = new FileRoot();


            foreach (var classDeclaration in input.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var name = classDeclaration.Identifier.ValueText;
                var key = outFile.TypeRepository.RegisterTypeDefinition(name, string.Empty, outFile.GetPath(), Enumerable.Empty<TypeParameter>());

                var definition = new ClassDefinition()
                {
                    Name = name,
                    TypeKey = key,
                    Modifiers = new List<Modifier>() { Modifier.Public }
                };

                outFile.Model.Items.Add(definition);
            }
        }
    }
}
