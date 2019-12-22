using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces
{
    public interface IScopeProvider<TAstNode>
    {
        ISubTreeScope GetSubTreeScope(TAstNode astNode);

        ISingleNodeScope GetSingleNodeScope(TAstNode astNode);

        NodePath GetNodePath(TAstNode astNode);

        TAstNode ResolveNode(NodePath nodePath);
    }
}
