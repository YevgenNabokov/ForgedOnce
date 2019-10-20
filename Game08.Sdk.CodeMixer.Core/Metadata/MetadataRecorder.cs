using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class MetadataRecorder : IMetadataRecorder
    {
        private readonly IMetadataWriter metadataWriter;
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private readonly string pluginId;

        public MetadataRecorder(IMetadataWriter metadataWriter, IPipelineExecutionInfo pipelineExecutionInfo, string pluginId)
        {
            this.metadataWriter = metadataWriter;
            this.pipelineExecutionInfo = pipelineExecutionInfo;
            this.pluginId = pluginId;
        }

        public void SymbolsBound<TNode1, TNode2>(
            ISemanticInfoProvider<TNode1> semanticInfoProvider1,
            TNode1 node1,
            ISemanticInfoProvider<TNode2> semanticInfoProvider2,
            TNode2 node2,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            var symbol1 = semanticInfoProvider1.GetImmediateUpstreamSymbol(node1);
            var symbol2 = semanticInfoProvider2.GetImmediateUpstreamSymbol(node2);

            if (symbol1 != null && symbol2 != null)
            {
                this.metadataWriter.Write(new Bound(symbol1, symbol2, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }
        }

        public void SymbolGenerated<TSubject>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            var symbols = semanticInfoProvider1.GetImmediateDownstreamSymbols(subject).ToArray();
            foreach (var symbol in symbols)
            {
                this.metadataWriter.Write(new Generated(symbol, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }

            if (symbols.Length == 0)
            {
                this.SymbolModified<TSubject>(semanticInfoProvider1, subject, new HashSet<string>());
            }
        }

        public void SymbolGenerated<TSubject, TFrom>(
            ISemanticInfoProvider<TSubject> semanticInfoProvider1,
            TSubject subject,
            ISemanticInfoProvider<TFrom> semanticInfoProvider2,
            TFrom from,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            var fromSymbol = semanticInfoProvider2.GetImmediateUpstreamSymbol(from);
            var symbols = semanticInfoProvider1.GetImmediateDownstreamSymbols(subject).ToArray();
            foreach (var symbol in symbols)
            {                
                this.metadataWriter.Write(new Generated(symbol, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags, fromSymbol));
            }

            if (symbols.Length == 0)
            {
                this.SymbolModified<TSubject>(semanticInfoProvider1, subject, new HashSet<string>());
            }
        }

        public void SymbolModified<TTarget>(
            ISemanticInfoProvider<TTarget> semanticInfoProvider1,
            TTarget target,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            var symbol = semanticInfoProvider1.GetImmediateUpstreamSymbol(target);
            if (symbol != null)
            {
                this.metadataWriter.Write(new Modified(symbol,this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }
        }

        public void SymbolSourcingFrom<TNode>(
            ISemanticInfoProvider<TNode> semanticInfoProvider1,
            TNode from,
            TNode subject,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            var fromSymbol = semanticInfoProvider1.GetImmediateUpstreamSymbol(from);
            if (fromSymbol != null)
            {
                foreach (var symbol in semanticInfoProvider1.GetImmediateDownstreamSymbols(subject))
                {
                    this.metadataWriter.Write(
                    new SourcingFrom(
                        fromSymbol,
                        symbol,
                        this.pipelineExecutionInfo.CurrentBatchIndex,
                        this.pipelineExecutionInfo.CurrentStageName,
                        this.pluginId,
                        pluginMetadata,
                        tags));
                }
            }
        }
    }
}
