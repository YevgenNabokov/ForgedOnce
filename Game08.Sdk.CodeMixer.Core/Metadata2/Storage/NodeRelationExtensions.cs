using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    public static class NodeRelationExtensions
    {
        public static Node GetOther(this NodeRelation rel, Node oneSide)
        {
            if (rel.Node1 != oneSide && rel.Node2 != oneSide)
            {
                throw new InvalidOperationException($"Node {oneSide} is not a part of relation {rel}.");
            }

            return oneSide == rel.Node1 ? rel.Node2 : rel.Node1;
        }
    }
}
