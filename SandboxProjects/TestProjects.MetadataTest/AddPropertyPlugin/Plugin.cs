﻿using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Parameters>
    {
        public const string PluginId = "70816D13-0C40-4092-9A28-498FE7A030D0";

        public const string OutStreamName = "PassThrough";

        public Plugin()
        {
            this.Signature = new ForgedOnce.Core.Plugins.PluginSignature()
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

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            var unit = SyntaxFactory.CompilationUnit();
            foreach (var nsUsage in input.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>())
            {
                unit = unit.AddUsings(nsUsage);
            }

            var nsContainer = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(this.Settings.OutputNamespace));

            foreach (var member in input.SyntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
            {
                nsContainer = nsContainer.AddMembers(member);
            }

            var outFile = this.Outputs[OutStreamName].CreateCodeFile(input.Name) as CodeFileCSharp;
            unit = unit.AddMembers(nsContainer);
            outFile.SyntaxTree = unit.SyntaxTree;

            PropertyAdder adder = new PropertyAdder(new Dictionary<string, string>()
            {
                { "AddedProp", "int" },
                { "OtherAddedProp", "string" },
            });
            var newRoot = adder.Visit(outFile.SyntaxTree.GetRoot());

            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);

            foreach (var added in outFile.SyntaxTree.GetRoot().DescendantNodes().Where((n) => n.GetAnnotations(adder.AnnotationKey).Any()))
            {
                metadataRecorder.SymbolGenerated<SyntaxNode>(outFile.NodePathService, added, new Dictionary<string, string>());
            }
        }
    }
}
