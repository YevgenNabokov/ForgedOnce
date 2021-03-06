﻿using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using ForgedOnce.Glsl;
using ForgedOnce.GlslLanguageServices.Builder;
using ForgedOnce.GlslLanguageServices.LanguageModels.Ast;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GlslPlugin
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
            result.Add(codeStreamFactory.CreateCodeStream(Languages.Glsl, OutStreamName));
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
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
