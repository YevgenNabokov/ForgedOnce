using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
{
    public class SemanticInfoProvider : ISemanticInfoProvider<Node>
    {
        private SemanticPathHelper semanticPathHelper = new SemanticPathHelper();

        private readonly CodeFileLtsModel codeFileLtsModel;

        public SemanticInfoProvider(CodeFileLtsModel codeFileLtsModel)
        {
            this.codeFileLtsModel = codeFileLtsModel;
        }

        public IEnumerable<ISemanticSymbol> GetImmediateDownstreamSymbols(Node astNode)
        {
            List<ISemanticSymbol> result = new List<ISemanticSymbol>();

            foreach (var path in this.semanticPathHelper.GetImmediateDownstreamPaths(astNode))
            {
                result.Add(new SemanticSymbol(this.codeFileLtsModel.LastRefreshBatchIndex, path));
            }

            return result;
        }

        public bool CanGetSymbolFor(Node astNode)
        {
            return this.semanticPathHelper.CanGetExactPathFor(astNode);
        }

        public ISemanticSymbol GetSymbolFor(Node astNode)
        {
            var path = this.semanticPathHelper.GetExactPath(astNode);
            return new SemanticSymbol(this.codeFileLtsModel.LastRefreshBatchIndex, path);
        }

        public ISemanticSymbol GetImmediateUpstreamSymbol(Node astNode)
        {
            var path = this.semanticPathHelper.GetImmediateUpstreamPath(astNode);
            if (path.Parts.Count == 0)
            {
                return null;
            }

            return new SemanticSymbol(this.codeFileLtsModel.LastRefreshBatchIndex, path);
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
