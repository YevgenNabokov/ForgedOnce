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
                    var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, link);
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
                        var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, link);
                        subjectNode.AddRecord(nodeRecord);
                        fromNode.AddRelation(link);
                        return;
                    }
                    else
                    {
                        var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
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
                    var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, ChangeKind.Created, null);
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
                    var nodeRecord = new NodeRecord(record.BatchIndex, record.StageName, record.PluginId, record.PluginMetadata, record.Tags, null, link);
                    subjectNode.AddRecord(nodeRecord);
                    fromNode.AddRelation(link);
                    return;
                }

                throw new NotSupportedException($"Unknown metadata record type {record.GetType()}.");
            }
        }

        public bool SymbolIsGeneratedBy(ISemanticSymbol symbol, ActivityFrame activityFrame, out NodeRecord recordMatch)
        {
            NodeRecord resultRecord = null;
            var node = this.GetExactNode(symbol, true);
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
                    foreach(var relation in relationGroup.Value)
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

        public IEnumerable<ISemanticSymbol> Lookup(MetadataLookupRequest request)
        {
            throw new NotImplementedException();
        }

        public void Refine(ISemanticSymbol symbol)
        {
            var exactNode = this.GetExactNode(symbol);
            var exactOrParentNode = this.GetExactNode(symbol, true);

            if (exactNode != null 
                && (exactNode.Relations.ContainsKey(RelationKind.SourcingFrom) || exactNode.Records.Any(r => r.Change == ChangeKind.Created)))
            {
                return;
            }

            var parentWithSourcing = this.FindParentWithKnownSourcing(exactOrParentNode);
            if (parentWithSourcing != null)
            {
                var sourcingRelation = parentWithSourcing.Relations[RelationKind.SourcingFrom].First();
                var sourceNode = sourcingRelation.GetOther(parentWithSourcing);
                var relativePath = symbol.SemanticPath.Parts.Skip(parentWithSourcing.GetPathLevelsFromRoot().Count());
                var mirrorPath = new SemanticPath(symbol.SemanticPath.Language, sourceNode.GetPathLevelsFromRoot().Concat(relativePath));
                var mirrorNode = sourceNode.RootIndex.GetExactNode(mirrorPath);
                if (mirrorNode != null)
                {
                    exactNode = exactNode ?? this.AllocateNode(symbol);
                    var newRelation = new NodeRelation(RelationKind.SourcingFrom, exactNode, mirrorNode);
                    var record = new NodeRecord(
                        sourcingRelation.ParentRecord.BatchIndex,
                        sourcingRelation.ParentRecord.StageName,
                        sourcingRelation.ParentRecord.PluginId,
                        sourcingRelation.ParentRecord.PluginMetadata,
                        sourcingRelation.ParentRecord.Tags,
                        sourcingRelation.ParentRecord.Change,
                        newRelation);
                    exactNode.AddRecord(record);
                    mirrorNode.AddRelation(newRelation);
                }
            }
        }

        public void Refresh()
        {
            
        }

        private Node FindParentWithKnownSourcing(Node node)
        {
            while (node != null)
            {
                if (node.Relations.ContainsKey(RelationKind.SourcingFrom) && node.Relations[RelationKind.SourcingFrom].Count > 0)
                {
                    return node;
                }

                if (node.Records.Any(r => r.Change == ChangeKind.Created))
                {
                    return null;
                }

                node = node.Parent;
            }

            return null;
        }

        private Node AllocateNode(ISemanticSymbol symbol)
        {
            return this.metadataIndexed.AllocateNode(symbol.SemanticPath);
        }

        private Node GetExactNode(ISemanticSymbol symbol, bool orParent = false)
        {
            return this.metadataIndexed.GetExactNode(symbol.SemanticPath, orParent);
        }
    }
}
