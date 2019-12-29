using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class MetadataStore : IMetadataWriter, IMetadataReader
    {
        public List<RecordBase> Records = new List<RecordBase>();

        private MetadataIndex metadataIndexed = new MetadataIndex();

        public bool NodeIsGeneratedBy(NodePath path, ActivityFrame activityFrame, out NodeRecord recordMatch)
        {
            NodeRecord resultRecord = null;
            var node = this.metadataIndexed.GetExactNode(path, true);
            if (node != null)
            {
                MetadataVerificationRequest request = new MetadataVerificationRequest(
                SearchMode.RelationsFirst,
                AncestrySearchDirection.ToParent,
                (n) =>
                {
                    foreach (var record in n.Records.Where(r => r.Change == ChangeKind.Created))
                    {
                        if ((activityFrame.PluginId == null || activityFrame.PluginId == record.PluginId)
                            && (activityFrame.StageName == null || activityFrame.StageName == record.StageName)
                            && (activityFrame.BatchIndex == null || activityFrame.BatchIndex == record.BatchIndex))
                        {
                            resultRecord = record;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return null;
                },
                (r, n) => (r.ParentRecord == null || activityFrame.BatchIndex == null || r.ParentRecord.BatchIndex <= activityFrame.BatchIndex) && r.Node1 == n,
                null,
                new HashSet<RelationKind>() { RelationKind.SourcingFrom }
                );

                var result = this.Verify(node, request);
                recordMatch = resultRecord;
                return result;
            }

            recordMatch = resultRecord;
            return false;
        }

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

        public bool Verify(Node node, MetadataVerificationRequest request)
        {
            var toCheckAncestry = new HashSet<Node>();
            var toCheckRelations = new HashSet<Node>();

            var visited = new HashSet<Node>();
            var currentBatch = new HashSet<Node>();
            currentBatch.Add(node);

            while (currentBatch.Count > 0 || toCheckRelations.Count > 0 || toCheckAncestry.Count > 0)
            {
                foreach (var n in currentBatch)
                {
                    visited.Add(n);
                    toCheckRelations.Add(n);
                    toCheckAncestry.Add(n);

                    var result = request.ResultEvaluator(n);
                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                }

                currentBatch.Clear();

                if (request.SearchMode == SearchMode.AncestryFirst)
                {
                    this.ExtractAncestry(toCheckAncestry, currentBatch, visited, request.AncestryNeighborSelector, request.AncestrySearchDirection);
                    toCheckAncestry.Clear();
                    if (currentBatch.Count == 0)
                    {
                        this.ExtractRelations(toCheckRelations, currentBatch, visited, request.RelationsToCheck, request.RelationSelector);
                        toCheckRelations.Clear();
                    }
                }
                else
                {
                    this.ExtractRelations(toCheckRelations, currentBatch, visited, request.RelationsToCheck, request.RelationSelector);
                    toCheckRelations.Clear();
                    if (currentBatch.Count == 0)
                    {
                        this.ExtractAncestry(toCheckAncestry, currentBatch, visited, request.AncestryNeighborSelector, request.AncestrySearchDirection);
                        toCheckAncestry.Clear();
                    }
                }
            }

            return false;
        }

        private void ExtractRelations(
            HashSet<Node> source,
            HashSet<Node> destination,
            HashSet<Node> excludeNodes,
            HashSet<RelationKind> relationKinds,
            Func<NodeRelation, Node, bool> selector)
        {
            if (relationKinds != null && relationKinds.Count == 0)
            {
                return;
            }

            foreach (var node in source)
            {
                foreach (var relationGroup in node.Relations.Where(r => relationKinds == null || relationKinds.Contains(r.Key)))
                {
                    foreach (var relation in relationGroup.Value)
                    {
                        if (selector == null || selector(relation, node))
                        {
                            var newNode = relation.GetOther(node);
                            if (!excludeNodes.Contains(newNode) && !destination.Contains(newNode))
                            {
                                destination.Add(newNode);
                            }
                        }
                    }
                }
            }
        }

        private void ExtractAncestry(
            HashSet<Node> source,
            HashSet<Node> destination,
            HashSet<Node> excludeNodes,
            Func<Node, bool> ancestryNeighborSelector,
            AncestrySearchDirection ancestrySearchDirection)
        {
            if (ancestrySearchDirection != AncestrySearchDirection.None)
            {
                foreach (var node in source)
                {
                    if (ancestrySearchDirection.HasFlag(AncestrySearchDirection.ToParent))
                    {
                        if (node.Parent != null
                            && (ancestryNeighborSelector == null || ancestryNeighborSelector(node.Parent))
                            && !excludeNodes.Contains(node.Parent)
                            && !destination.Contains(node.Parent))
                        {
                            destination.Add(node.Parent);
                        }
                    }

                    if (ancestrySearchDirection.HasFlag(AncestrySearchDirection.ToChild))
                    {
                        foreach (var child in node.Children.Values)
                        {
                            if ((ancestryNeighborSelector == null || ancestryNeighborSelector(child))
                            && !excludeNodes.Contains(child)
                            && !destination.Contains(child))
                            {
                                destination.Add(child);
                            }
                        }
                    }
                }
            }
        }

        public void Commit()
        {
            foreach (var record in this.Records.Where(r => !(r is SourcingFrom)))
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

                throw new NotSupportedException($"Unknown metadata record type {record.GetType()}.");
            }

            foreach (var record in this.Records.OfType<SourcingFrom>())
            {
                this.CommitSourcingFromChange(record);
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
                relations.AddRange(this.RefineSourcingFromRelations(fromNode, subjectNode, record.Subject));
            }
            
            var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, relations.ToArray());
            foreach (var relation in relations)
            {
                relation.Node1.AddRecord(nodeRecord);
                relation.Node2.AddRelation(relation);
            }
        }

        protected IEnumerable<NodeRelation> RefineSourcingFromRelations(Node source, Node subject, ISubTreeSnapshot subjectSnapshot)
        {
            List<NodeRelation> result = new List<NodeRelation>();
            foreach (var c in source.Children)
            {
                this.Refine(c.Value, source.GetPathLevelsFromRoot().ToArray(), subject.GetPathLevelsFromRoot().ToArray(), subjectSnapshot, result);
            }

            return result;
        }

        protected void Refine(Node source, NodePathLevel[] sourceRootLevels, NodePathLevel[] subjectRootLevels, ISubTreeSnapshot subjectSnapshot, List<NodeRelation> result)
        {
            if (source.Records.Count > 0)
            {
                var sourceLevels = source.GetPathLevelsFromRoot().ToArray();
                NodePath path = new NodePath(source.Language, subjectRootLevels.Concat(NodePath.RelativeTo(sourceLevels, sourceRootLevels)));
                if (subjectSnapshot.ContainsNode(path))
                {
                    result.Add(new NodeRelation(RelationKind.SourcingFrom, this.metadataIndexed.AllocateNode(path), source));
                }
                else
                {
                    return;
                }
            }

            foreach (var c in source.Children)
            {
                this.Refine(c.Value, sourceRootLevels, subjectRootLevels, subjectSnapshot, result);
            }
        }
    }
}
