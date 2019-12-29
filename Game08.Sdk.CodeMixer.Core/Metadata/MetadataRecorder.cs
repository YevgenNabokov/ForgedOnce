using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
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
            INodePathService<TNode1> nodePathService1,
            TNode1 node1,
            INodePathService<TNode2> nodePathService2,
            TNode2 node2,
            IDictionary<string, string> tags,
            object pluginMetadata = null)
        {
            var snapshot1 = nodePathService1.GetSingleNodeSnapshot(node1);
            var snapshot2 = nodePathService2.GetSingleNodeSnapshot(node2);

            if (snapshot1 != null && snapshot2 != null)
            {
                this.metadataWriter.Write(new Bound(snapshot1, snapshot2, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
            }
        }

        public void SymbolGenerated<TSubject>(
            INodePathService<TSubject> nodePathService1,
            TSubject subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null)
        {
            var symbol = nodePathService1.GetSubTreeSnapshot(subject);
            this.metadataWriter.Write(new Generated(symbol, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
        }

        public void SymbolGenerated<TSubject, TFrom>(
            INodePathService<TSubject> nodePathService1,
            TSubject subject,
            INodePathService<TFrom> nodePathService2,
            TFrom from,
            IDictionary<string, string> tags,
            object pluginMetadata = null)
        {
            var snapshotFrom = nodePathService2.GetSingleNodeSnapshot(from);
            var snapshotSubject = nodePathService1.GetSubTreeSnapshot(subject);
            this.metadataWriter.Write(new Generated(snapshotSubject, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags, snapshotFrom));
        }

        public void SymbolModified<TTarget>(
            INodePathService<TTarget> nodePathService1,
            TTarget target,
            IDictionary<string, string> tags,
            object pluginMetadata = null)
        {
            var snapshotTarget = nodePathService1.GetSingleNodeSnapshot(target);
            this.metadataWriter.Write(new Modified(snapshotTarget, this.pipelineExecutionInfo.CurrentBatchIndex, this.pipelineExecutionInfo.CurrentStageName, this.pluginId, pluginMetadata, tags));
        }

        public void SymbolSourcingFrom<TNode>(
            INodePathService<TNode> nodePathService1,
            TNode from,
            INodePathService<TNode> nodePathService2,
            TNode subject,
            IDictionary<string, string> tags,
            object pluginMetadata = null)
        {
            var snapshotFrom = nodePathService1.GetSingleNodeSnapshot(from);
            var snapshotSubject = nodePathService2.GetSubTreeSnapshot(subject);

            this.metadataWriter.Write(
                    new SourcingFrom(
                        snapshotFrom,
                        snapshotSubject,
                        this.pipelineExecutionInfo.CurrentBatchIndex,
                        this.pipelineExecutionInfo.CurrentStageName,
                        this.pluginId,
                        pluginMetadata,
                        tags));
        }
    }
}
