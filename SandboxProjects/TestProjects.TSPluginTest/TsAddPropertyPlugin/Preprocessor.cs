using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Storage;
using ForgedOnce.Core.Plugins;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TypeScript;
using ForgedOnce.TypeScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TsAddPropertyPlugin
{
    public class Preprocessor : IPluginPreprocessor<CodeFileTs, Parameters, Settings>
    {
        public Parameters GenerateParameters(CodeFileTs input, Settings pluginSettings, IMetadataReader metadataReader, ILogger logger, IFileGroup<CodeFileTs, GroupItemDetails> fileGroup = null)
        {
            var searcher = new SearchVisitor();

            var classDeclarations = searcher.FindNodes<StClassDeclaration>(input.Model, c => true).ToArray();

            return new Parameters()
            {
                AdditionalProperty = classDeclarations.Any(d => metadataReader.NodeIsGeneratedBy(input.NodePathService.GetNodePath(d), new ActivityFrame(pluginId: "TsTestPlugin"), out _))
            };
        }
    }
}
