using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Storage;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class AddedPropsPreprocessor : IPluginPreprocessor<CodeFileCSharp, Parameters, Settings>
    {
        public Parameters GenerateMetadata(CodeFileCSharp input, Settings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<CodeFileCSharp, GroupItemDetails> fileGroup = null)
        {
            var cSharpFile = input as CodeFileCSharp;

            var propertyAdderActivity = new ActivityFrame(pluginId: AddPropertyPlugin.Plugin.PluginId);

            NodeRecord record;

            return new Parameters()
            {
                PropertiesToDecorate = cSharpFile
                .SyntaxTree
                .GetRoot()
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(d => metadataReader.NodeIsGeneratedBy(cSharpFile.NodePathService.GetNodePath(d), propertyAdderActivity, out record))
            };
        }
    }
}
