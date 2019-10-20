using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.CSharp;
using Game08.Sdk.CodeMixer.Glsl;
using Game08.Sdk.GlslLanguageServices.Builder;
using Game08.Sdk.GlslLanguageServices.LanguageModels.Ast;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GlslPlugin
{
    public class GlslTestPluginImplementation : CodeGenerationFromCSharpPlugin<GlslTestPluginSettings, GlslTestPluginMetadata>
    {
        public const string OutStreamName = "PassThrough";

        public GlslTestPluginImplementation()
        {
            this.Signature = new Game08.Sdk.CodeMixer.Core.Plugins.PluginSignature()
            {
                Id = new Guid().ToString(),
                InputLanguage = Languages.CSharp,
                Name = nameof(GlslTestPluginImplementation)
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            result.Add(codeStreamFactory.CreateCodeStream(Languages.Glsl, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, GlslTestPluginMetadata inputParameters, IMetadataRecorder metadataRecorder)
        {
            var outFile = this.Outputs[OutStreamName].CreateCodeFile($"{Path.GetFileNameWithoutExtension(input.Name)}.vertex.glsl") as CodeFileGlsl;
            outFile.ShaderFile = ShaderFile.CreateEmpty();
            outFile.ShaderFile.SyntaxTree.Version = ShaderVersion.GlslEs300;

            outFile.ShaderFile.SyntaxTree.Declarations.Add(new FunctionDeclaration()
            {
                Name = new Identifier() { Name = "main" },
                TypeSpecifier = new TypeNameSpecifier() { Identifier = new Identifier() { Name = "void" } },
                Statement = new StatementCompound()
            });
        }
    }
}
