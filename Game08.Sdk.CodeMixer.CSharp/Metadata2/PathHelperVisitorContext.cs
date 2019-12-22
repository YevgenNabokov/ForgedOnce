using Game08.Sdk.CodeMixer.Core.Metadata2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class PathHelperVisitorContext
    {
        public PathHelperVisitorContext(
            object searchedNode,
            string annotationId,
            bool annotateOnlySearchedNode,
            NodePathLevel[] startLevels = null,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> map = null)
        {
            this.SearchedNode = searchedNode;
            this.Map = map ?? PathHelperVisitorMap.Map;
            this.CurrentPath = new Stack<NodePathLevel>();
            this.AnnotationId = annotationId;
            this.AnnotateOnlySearchedNode = annotateOnlySearchedNode;
            if (startLevels != null)
            {
                foreach (var level in startLevels)
                {
                    this.CurrentPath.Push(level);
                }
            }
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, PathHelperVisitorMapBranchInfo>> Map { get; private set; }

        public Stack<NodePathLevel> CurrentPath { get; private set; }

        public object SearchedNode { get; private set; }

        public bool AnnotateOnlySearchedNode { get; private set; }

        public NodePath SearchedNodePath { get; set; }

        public bool IsOnOrBelowSearchedNode { get; set; }

        public string AnnotationId { get; private set; }

        public bool ShouldAnnotate()
        {
            return !string.IsNullOrEmpty(this.AnnotationId) && this.IsOnOrBelowSearchedNode;
        }
    }
}
