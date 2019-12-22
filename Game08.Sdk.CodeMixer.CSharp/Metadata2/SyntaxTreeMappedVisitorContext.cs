using Game08.Sdk.CodeMixer.Core.Metadata2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class SyntaxTreeMappedVisitorContext
    {
        public SyntaxTreeMappedVisitorContext(
            NodePathLevel[] startLevels = null,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> map = null)
        {
            this.CurrentPath = new Stack<NodePathLevel>();
            this.Map = map ?? PathHelperVisitorMap.Map;
            if (startLevels != null)
            {
                foreach (var level in startLevels)
                {
                    this.CurrentPath.Push(level);
                }
            }
        }

        public Stack<NodePathLevel> CurrentPath { get; private set; }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> Map { get; private set; }
    }
}
