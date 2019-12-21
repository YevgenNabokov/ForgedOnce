using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    public class NodeRelation
    {
        public NodeRelation(RelationKind relationKind, Node node1, Node node2)
        {
            this.RelationKind = relationKind;
            this.Node1 = node1;
            this.Node2 = node2;
            if (node1 == node2)
            {
                throw new InvalidOperationException("Node relation cannot have the same node on both sides.");
            }
        }

        public RelationKind RelationKind { get; private set; }

        public Node Node1 { get; private set; }

        public Node Node2 { get; private set; }

        public NodeRecord ParentRecord { get; set; }
    }
}
