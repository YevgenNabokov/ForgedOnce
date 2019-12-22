using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2
{
    public class NodePath
    {
        public NodePath(string language, IEnumerable<NodePathLevel> levels)
        {
            this.Language = language;
            this.Levels = levels.ToArray();
        }

        public string Language { get; private set; }

        public IReadOnlyList<NodePathLevel> Levels { get; private set; }

        public override string ToString()
        {
            return PartsToString(this.Language, this.Levels);
        }        

        public static NodePath FromString(string pathString)
        {
            var parts = pathString.Split(':');
            return new NodePath(
                parts[0],
                parts.Length > 1
                ? parts[1].Split('/').Select(p => NodePathLevel.FromString(p))
                : Enumerable.Empty<NodePathLevel>());
        }

        public static string PartsToString(string language, IEnumerable<NodePathLevel> levels)
        {
            return $"{language}:{LevelsToString(levels)}";
        }

        public static string LevelsToString(IEnumerable<NodePathLevel> levels)
        {
            return string.Join("/", levels.Select(l => l.ToString()));
        }

        public override bool Equals(object obj)
        {
            var path = obj as NodePath;
            if (path != null)
            {
                return path.Language == this.Language && path.Levels.SequenceEqual(this.Levels);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (this.Language != null ? this.Language.GetHashCode() : 0);
                foreach (var l in this.Levels)
                {
                    hash = hash * 23 + l.GetHashCode();
                }

                return hash;
            }
        }

        public bool StartsWith(NodePath other)
        {
            ValidateOtherPathArgument(other, nameof(other));

            if (other.Levels.Count > this.Levels.Count)
            {
                return false;
            }

            for (var i = 0; i < other.Levels.Count; i++)
            {
                if (!other.Levels[i].Equals(this.Levels[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public NodePath Concat(NodePath other)
        {
            ValidateOtherPathArgument(other, nameof(other));

            return new NodePath(this.Language, this.Levels.Concat(other.Levels));
        }

        public NodePath RelativeTo(NodePath other)
        {
            ValidateOtherPathArgument(other, nameof(other));

            if (other.Levels.Count > this.Levels.Count)
            {
                return null;
            }

            List<NodePathLevel> nodePathLevels = new List<NodePathLevel>();
            for (var i = 0; i < this.Levels.Count; i++)
            {
                if (other.Levels.Count > i)
                {
                    if (!other.Levels[i].Equals(this.Levels[i]))
                    {
                        return null;
                    }
                }
                else
                {
                    nodePathLevels.Add(this.Levels[i]);
                }
            }

            return new NodePath(this.Language, nodePathLevels);
        }

        public NodePath CommonRoot(NodePath other)
        {
            ValidateOtherPathArgument(other, nameof(other));

            List<NodePathLevel> nodePathLevels = new List<NodePathLevel>();
            for (var i = 0; i < Math.Min(this.Levels.Count, other.Levels.Count); i++)
            {
                if (this.Levels[i].Equals(other.Levels[i]))
                {
                    nodePathLevels.Add(this.Levels[i]);
                }
                else
                {
                    break;
                }
            }

            return new NodePath(this.Language, nodePathLevels);
        }

        protected void ValidateOtherPathArgument(NodePath other, string parameterName)
        {
            if (other.Language != this.Language)
            {
                throw new ArgumentException($"Provided path should have the same {nameof(Language)}.", parameterName);
            }
        }
    }
}
