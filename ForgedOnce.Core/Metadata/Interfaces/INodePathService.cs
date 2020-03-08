using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Interfaces
{
    public interface INodePathService<TAstNode>
    {
        ISubTreeSnapshot GetSubTreeSnapshot(TAstNode astNode);

        ISingleNodeSnapshot GetSingleNodeSnapshot(TAstNode astNode);

        NodePath GetNodePath(TAstNode astNode);

        TAstNode ResolveNode(NodePath nodePath);
    }
}
