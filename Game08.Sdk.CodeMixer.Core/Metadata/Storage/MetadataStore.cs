using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class MetadataStore : IMetadataWriter, IMetadataReader
    {
        public List<RecordBase> Records = new List<RecordBase>();

        private Dictionary<int, MetadataIndex> metadataIndexed = new Dictionary<int, MetadataIndex>();

        public void Write(RecordBase record)
        {
            if (record != null)
            {
                this.Records.Add(record);

                if (record is Bound)
                {
                    var bound = (Bound)record;

                    if (bound.Item1 == null || bound.Item2 == null)
                    {
                        throw new InvalidOperationException("Cannot add nodes Bound metadata record, one or both symbols are null.");
                    }

                    var node1 = this.AllocateNode(bound.Item1);
                    var node2 = this.AllocateNode(bound.Item2);
                    var link = new NodeRelation(RelationKind.Bound, node1, node2);
                    var nodeRecord = new NodeRecord(record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, link);
                    node1.AddRecord(nodeRecord);
                    node2.AddRecord(nodeRecord);
                    return;
                }

                if (record is Generated)
                {
                    var generated = (Generated)record;

                    if (generated.Subject == null)
                    {
                        throw new InvalidOperationException("Cannot add node Generated metadata, subject symbol is null.");
                    }

                    var subjectNode = this.AllocateNode(generated.Subject);
                    if (generated.From != null)
                    {
                        var fromNode = this.AllocateNode(generated.From);
                        var link = new NodeRelation(RelationKind.GeneratedFrom, subjectNode, fromNode);
                        var nodeRecord = new NodeRecord(record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, link);
                        subjectNode.AddRecord(nodeRecord);
                        fromNode.AddRelation(link);
                        return;
                    }
                    else
                    {
                        var nodeRecord = new NodeRecord(record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
                        subjectNode.AddRecord(nodeRecord);
                        return;
                    }
                }

                if (record is Modified)
                {
                    var modified = (Modified)record;

                    if (modified.Target == null)
                    {
                        throw new InvalidOperationException("Cannot add node Modified metadata, target symbol is null.");
                    }

                    var targetNode = this.AllocateNode(modified.Target);
                    var nodeRecord = new NodeRecord(record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
                    targetNode.AddRecord(nodeRecord);
                    return;
                }

                if (record is SourcingFrom)
                {
                    var sourcingFrom = (SourcingFrom)record;

                    if (sourcingFrom.Subject == null || sourcingFrom.From == null)
                    {
                        throw new InvalidOperationException("Cannot add nodes SourcingFrom metadata record, one or both symbols are null.");
                    }

                    var subjectNode = this.AllocateNode(sourcingFrom.Subject);
                    var fromNode = this.AllocateNode(sourcingFrom.From);
                    var link = new NodeRelation(RelationKind.SourcingFrom, subjectNode, fromNode);
                    var nodeRecord = new NodeRecord(record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, link);
                    subjectNode.AddRecord(nodeRecord);
                    fromNode.AddRelation(link);
                    return;
                }

                throw new NotSupportedException($"Unknown metadata record type {record.GetType()}.");
            }
        }

        public void Refresh()
        {
            
        }

        private Node AllocateNode(ISemanticSymbol symbol)
        {
            if (!this.metadataIndexed.ContainsKey(symbol.BatchIndex))
            {
                this.metadataIndexed.Add(symbol.BatchIndex, new MetadataIndex(symbol.BatchIndex));
            }

            return this.metadataIndexed[symbol.BatchIndex].AllocateNode(symbol.SemanticPath);
        }
    }
}
