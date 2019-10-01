using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class Node
    {
        private List<NodeRecord> records = new List<NodeRecord>();

        private Dictionary<PathLevel, Node> children = new Dictionary<PathLevel, Node>();

        private Dictionary<RelationKind, List<NodeRelation>> relations = new Dictionary<RelationKind, List<NodeRelation>>();

        private List<PathLevel> pathLevels = new List<PathLevel>();

        public Node(Node parent, string language, MetadataIndex rootIndex, IEnumerable<PathLevel> pathLevels)
        {
            this.pathLevels = new List<PathLevel>(pathLevels);

            if (this.pathLevels.Count == 0)
            {
                throw new InvalidOperationException("Node should have more than 0 path levels.");
            }

            if (parent != null)
            {
                parent.children.Add(this.pathLevels[0], this);
            }

            this.RootIndex = rootIndex;
            this.Language = language;
        }        

        public MetadataIndex RootIndex { get; private set; }

        public string Language { get; private set; }

        public IReadOnlyDictionary<PathLevel, Node> Children
        {
            get
            {
                return this.children;
            }
        }

        public IReadOnlyList<PathLevel> PathLevels
        {
            get
            {
                return this.pathLevels;
            }
        }

        public Node Parent { get; private set; }

        public IReadOnlyList<NodeRecord> Records
        {
            get
            {
                return this.records;
            }
        }

        public IReadOnlyDictionary<RelationKind, List<NodeRelation>> Relations
        {
            get
            {
                return this.relations;
            }
        }

        public Node Split(int pathLevelIndex)
        {
            if (pathLevelIndex < 0 || pathLevelIndex > this.PathLevels.Count - 2)
            {
                throw new InvalidOperationException($"Cannot split node: {nameof(pathLevelIndex)} must be greater than zero and less than current level count minus one.");
            }

            if (this.Parent != null)
            {
                this.Parent.children.Remove(this.pathLevels[0]);
            }

            var result = new Node(this.Parent, this.Language, this.RootIndex, this.pathLevels.Take(pathLevelIndex + 1));
            this.pathLevels = new List<PathLevel>(this.pathLevels.Skip(pathLevelIndex + 1));

            result.children.Add(this.pathLevels[0], this);
            this.Parent = result;

            return result;
        }
    }
}
