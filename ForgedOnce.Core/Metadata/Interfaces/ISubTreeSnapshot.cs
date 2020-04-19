using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Interfaces
{
    public interface ISubTreeSnapshot : ISnapshot
    {
        MetadataRoot[] ResolveRoots();

        bool ContainsNode(NodePath path);

        NodePath GetNodeOriginalPath(NodePath currentPath);
    }
}
