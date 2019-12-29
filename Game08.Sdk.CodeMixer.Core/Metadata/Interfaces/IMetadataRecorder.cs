using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Interfaces
{
    public interface IMetadataRecorder
    {
        void SymbolsBound<TNode1, TNode2>(
            INodePathService<TNode1> subTreeScopeProvider1,
            TNode1 node1,
            INodePathService<TNode2> subTreeScopeProvider2,
            TNode2 node2,
            IDictionary<string, string> tags,
            object pluginMetadata = null
            );

        void SymbolGenerated<TSubject>(
            INodePathService<TSubject> subTreeScopeProvider1,
            TSubject subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolGenerated<TSubject, TFrom>(
            INodePathService<TSubject> subTreeScopeProvider1,
            TSubject subject,
            INodePathService<TFrom> subTreeScopeProvider2,
            TFrom from,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolModified<TTarget>(
            INodePathService<TTarget> subTreeScopeProvider1,
            TTarget target,
            IDictionary<string, string> tags,
            object pluginMetadata = null);

        void SymbolSourcingFrom<TNode>(
            INodePathService<TNode> subTreeScopeProvider1,
            TNode from,
            INodePathService<TNode> subTreeScopeProvider2,
            TNode subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null);
    }
}
