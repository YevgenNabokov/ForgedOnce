using ForgedOnce.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript.Metadata
{
    public class SyntaxTreeMappedVisitorContext
    {
        public SyntaxTreeMappedVisitorContext(
            NodePathLevel[] startLevels = null,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> map = null)
        {
            this.CurrentPath = new Stack<NodePathLevel>();
            this.Map = map ?? SyntaxTreeMapper.Map;
            if (startLevels != null)
            {
                foreach (var level in startLevels)
                {
                    this.CurrentPath.Push(level);
                }
            }
        }

        public Stack<NodePathLevel> CurrentPath { get; private set; }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, SyntaxTreeMapBranchInfo>> Map { get; private set; }
    }
}
