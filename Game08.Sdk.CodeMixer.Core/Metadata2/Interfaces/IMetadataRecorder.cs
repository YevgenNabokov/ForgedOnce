using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces
{
    public interface IMetadataRecorder
    {
        void SymbolsBound<TNode1, TNode2>(
            IScopeProvider<TNode1> subTreeScopeProvider1,
            TNode1 node1,
            IScopeProvider<TNode2> subTreeScopeProvider2,
            TNode2 node2,
            IDictionary<string, string> tags,
            object pluginMetadata = null
            );

        void SymbolGenerated<TSubject>(
            IScopeProvider<TSubject> subTreeScopeProvider1,
            TSubject subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolGenerated<TSubject, TFrom>(
            IScopeProvider<TSubject> subTreeScopeProvider1,
            TSubject subject,
            IScopeProvider<TFrom> subTreeScopeProvider2,
            TFrom from,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolModified<TTarget>(
            IScopeProvider<TTarget> subTreeScopeProvider1,
            TTarget target,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolSourcingFrom<TNode>(
            IScopeProvider<TNode> subTreeScopeProvider1,
            TNode from,
            IScopeProvider<TNode> subTreeScopeProvider2,
            TNode subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);
    }
}
