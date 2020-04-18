using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class Preprocessor : IPluginPreprocessor<CodeFileCSharp, Parameters, Settings>
    {
        public Parameters GenerateParameters(CodeFileCSharp input, Settings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<CodeFileCSharp, GroupItemDetails> fileGroup = null)
        {
            var result = new Parameters();

            foreach (var classDeclaration in input.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var declaredSymbol = input.SemanticModel.GetDeclaredSymbol(classDeclaration);
                var typeName = declaredSymbol.GetFullMetadataName();
                List<string> names = new List<string>();
                foreach (var name in pluginSettings.PropertyNames)
                {
                    if (declaredSymbol.GetMembers(name).Any())
                    {
                        if (!pluginSettings.SkipPropertyNameIfConflicts)
                        {
                            throw new InvalidOperationException($"Target class {typeName} already contains member named as {name}.");
                        }
                    }
                    else
                    {
                        names.Add(name);
                    }
                }

                result.TypePropertyNames.Add(typeName, names.ToArray());
            }

            return result;
        }
    }
}
