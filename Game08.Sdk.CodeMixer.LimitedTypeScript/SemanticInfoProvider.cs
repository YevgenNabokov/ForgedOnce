using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class SemanticInfoProvider : ISemanticInfoProvider<Node>
    {
        private SemanticPathBuilder semanticPathBuilder = new SemanticPathBuilder();

        private readonly CodeFileLtsModel codeFileLtsModel;

        public SemanticInfoProvider(CodeFileLtsModel codeFileLtsModel)
        {
            this.codeFileLtsModel = codeFileLtsModel;
        }

        public ISemanticSymbol GetSymbol(Node astNode)
        {
            var path = this.semanticPathBuilder.Build(astNode);

            throw new NotImplementedException();
        }

        public bool Resolve(ISemanticSymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
