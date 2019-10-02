using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class NodeRelation
    {
        public NodeRelation(RelationKind relationKind, Node node1, Node node2)
        {
            this.RelationKind = relationKind;
            this.Node1 = node1;
            this.Node2 = node2;
        }

        public RelationKind RelationKind { get; private set; }

        public Node Node1 { get; private set; }

        public Node Node2 { get; private set; }

        public NodeRecord ParentRecord { get; set; }
    }
}
