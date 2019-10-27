using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Interfaces
{
    public interface IMetadataRecorder
    {
        void SymbolsBound<TNode1, TNode2>(
            ISemanticInfoProvider<TNode1> semanticInfoProvider1,
            TNode1 node1,
            ISemanticInfoProvider<TNode2> semanticInfoProvider2,
            TNode2 node2,
            IDictionary<string, string> tags,
            object pluginMetadata = null
            );

        void SymbolGenerated<TSubject>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolGenerated<TSubject, TFrom>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            ISemanticInfoProvider<TFrom> semanticInfoProvider2,
            TFrom from,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolModified<TTarget>(
            ISemanticInfoProvider<TTarget> semanticInfoProvider1,
            TTarget target,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolSourcingFrom<TNode>(
            ISemanticInfoProvider<TNode> semanticInfoProvider1,
            TNode from,
            ISemanticInfoProvider<TNode> semanticInfoProvider2,
            TNode subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);
    }
}
