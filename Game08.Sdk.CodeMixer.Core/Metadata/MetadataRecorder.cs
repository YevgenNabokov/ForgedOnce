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
                this.metadataWriter.Write(new Bound(symbol1, symbol2, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
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
                this.metadataWriter.Write(new Generated(symbol, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
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
                this.metadataWriter.Write(new Generated(symbol, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags, fromSymbol));
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
                this.metadataWriter.Write(new Modified(symbol, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }
        }

        public void SymbolMoved<TNode>(
            ISemanticInfoProvider<TNode> semanticInfoProvider1,
            TNode from,
            TNode to,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            if (semanticInfoProvider1.CanGetSymbolFor(from) && semanticInfoProvider1.CanGetSymbolFor(to))
            {
                this.metadataWriter.Write(
                    new Moved(
                        semanticInfoProvider1.GetSymbolFor(from),
                        semanticInfoProvider1.GetSymbolFor(to),
                        this.pipelineExecutionInfo.CurrentStageName,
                        this.pluginId,
                        pluginMetadata,
                        tags));
            }
            else
            {
                if (!semanticInfoProvider1.CanGetSymbolFor(from))
                {
                    this.SymbolModified<TNode>(semanticInfoProvider1, from, new HashSet<string>());
                }
                else
                {
                    this.SymbolRemoved(semanticInfoProvider1, from, new HashSet<string>());
                }

                if (!semanticInfoProvider1.CanGetSymbolFor(to))
                {
                    this.SymbolModified<TNode>(semanticInfoProvider1, to, new HashSet<string>());
                }
                else
                {
                    this.SymbolGenerated(semanticInfoProvider1, to, new HashSet<string>());
                }
            }
        }

        public void SymbolRemoved<TTarget>(
            ISemanticInfoProvider<TTarget> semanticInfoProvider1,
            TTarget target,
            HashSet<string> tags,
            object pluginMetadata = null)
        {
            if (semanticInfoProvider1.CanGetSymbolFor(target))
            {
                this.metadataWriter.Write(new Removed(semanticInfoProvider1.GetSymbolFor(target), this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }
            else
            {
                this.SymbolModified(semanticInfoProvider1, target, new HashSet<string>());
            }
        }
    }
}
