using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata2.Changes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    public class MetadataStore : IMetadataWriter2
    {
        public List<RecordBase> Records = new List<RecordBase>();

        private MetadataIndex metadataIndexed = new MetadataIndex();

        public void Write(RecordBase record)
        {
            if (record != null)
            {
                if (record is Bound)
                {
                    var bound = (Bound)record;

                    if (bound.Item1 == null || bound.Item2 == null)
                    {
                        throw new InvalidOperationException("Cannot add nodes Bound metadata record, one or both symbols are null.");
                    }
                }

                if (record is Generated)
                {
                    var generated = (Generated)record;

                    if (generated.Subject == null)
                    {
                        throw new InvalidOperationException("Cannot add node Generated metadata, subject symbol is null.");
                    }
                }

                if (record is Modified)
                {
                    var modified = (Modified)record;

                    if (modified.Target == null)
                    {
                        throw new InvalidOperationException("Cannot add node Modified metadata, target symbol is null.");
                    }
                }

                if (record is SourcingFrom)
                {
                    var sourcingFrom = (SourcingFrom)record;

                    if (sourcingFrom.Subject == null || sourcingFrom.From == null)
                    {
                        throw new InvalidOperationException("Cannot add nodes SourcingFrom metadata record, one or both symbols are null.");
                    }
                }

                this.Records.Add(record);
            }
        }

        public void Commit()
        {
            foreach (var record in this.Records)
            {
                if (record is Bound)
                {
                    this.CommitBoundChange((Bound)record);
                    continue;
                }

                if (record is Generated)
                {
                    this.CommitGeneratedChange((Generated)record);
                    continue;
                }

                if (record is Modified)
                {
                    this.CommitModifiedChange((Modified)record);
                    continue;
                }

                if (record is SourcingFrom)
                {
                    this.CommitSourcingFromChange((SourcingFrom)record);
                    continue;
                }

                throw new NotSupportedException($"Unknown metadata record type {record.GetType()}.");
            }

            this.Records.Clear();
        }

        protected void CommitBoundChange(Bound record)
        {
            var root1 = record.Item1.ResolveRoot();
            var node1 = this.metadataIndexed.AllocateNode(root1.CurrentPath);
            var root2 = record.Item2.ResolveRoot();
            var node2 = this.metadataIndexed.AllocateNode(root2.CurrentPath);

            var link = new NodeRelation(RelationKind.Bound, node1, node2);
            var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, new NodeRelation[] { link });
            node1.AddRecord(nodeRecord);
            node2.AddRecord(nodeRecord);
        }

        protected void CommitGeneratedChange(Generated record)
        {
            var subjectRoots = record.Subject.ResolveRoots();
            var subjectNodes = new List<Node>();
            foreach (var r in subjectRoots)
            {
                subjectNodes.Add(this.metadataIndexed.AllocateNode(r.CurrentPath));
            }
            if (record.From != null)
            {
                NodeRelation[] relations = new NodeRelation[subjectNodes.Count];
                var fromNode = this.metadataIndexed.AllocateNode(record.From.ResolveRoot().CurrentPath);
                for (var i = 0; i < subjectNodes.Count; i++)
                {
                    relations[i] = new NodeRelation(RelationKind.GeneratedFrom, subjectNodes[i], fromNode);
                }

                var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, relations);
                foreach (var n in subjectNodes)
                {
                    n.AddRecord(nodeRecord);
                }

                foreach (var link in relations)
                {
                    fromNode.AddRelation(link);
                }
            }
            else
            {
                var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
                foreach (var n in subjectNodes)
                {
                    n.AddRecord(nodeRecord);
                }
            }
        }

        protected void CommitModifiedChange(Modified record)
        {
            var targetNode = this.metadataIndexed.AllocateNode(record.Target.ResolveRoot().CurrentPath);
            var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
            targetNode.AddRecord(nodeRecord);
        }

        protected void CommitSourcingFromChange(SourcingFrom record)
        {
            var subjectRoots = record.Subject.ResolveRoots();
            var fromRoot = record.From.ResolveRoot();
            MetadataRoot subjectOriginalRoot = null;
            foreach (var root in subjectRoots)
            {
                if (subjectOriginalRoot == null)
                {
                    subjectOriginalRoot = root;
                }
                else
                {
                    if (subjectOriginalRoot.OriginalPath.Levels.Count > root.OriginalPath.Levels.Count)
                    {
                        subjectOriginalRoot = root;
                    }
                }
            }

            List<NodeRelation> relations = new List<NodeRelation>();
            foreach (var r in subjectRoots)
            {
                var sourcePath = fromRoot.CurrentPath.Concat(r.OriginalPath.RelativeTo(subjectOriginalRoot.OriginalPath));
                var fromNode = this.metadataIndexed.AllocateNode(sourcePath);
                var subjectNode = this.metadataIndexed.AllocateNode(r.CurrentPath);
                relations.Add(new NodeRelation(RelationKind.SourcingFrom, subjectNode, fromNode));
            }
            
            var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, relations.ToArray());
            foreach (var relation in relations)
            {
                relation.Node1.AddRecord(nodeRecord);
                relation.Node2.AddRelation(relation);
            }
        }
    }
}
