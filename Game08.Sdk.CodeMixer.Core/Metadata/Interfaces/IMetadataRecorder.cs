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
            HashSet<string> tags,
            object pluginMetadata = null
            );

        void SymbolGenerated<TSubject>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            HashSet<string> tags,
            object pluginMetadata = null);

        void SymbolGenerated<TSubject, TFrom>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            ISemanticInfoProvider<TFrom> semanticInfoProvider2,
            TFrom from,
            HashSet<string> tags,
            object pluginMetadata = null);

        void SymbolModified<TTarget>(
            ISemanticInfoProvider<TTarget> semanticInfoProvider1,
            TTarget target,
            HashSet<string> tags,
            object pluginMetadata = null);

        void SymbolMoved<TNode>(
            ISemanticInfoProvider<TNode> semanticInfoProvider1,
            TNode from,
            TNode to,
            HashSet<string> tags,
            object pluginMetadata = null);

        void SymbolRemoved<TTarget>(
            ISemanticInfoProvider<TTarget> semanticInfoProvider1,
            TTarget target,
            HashSet<string> tags,
            object pluginMetadata = null);
    }
}
