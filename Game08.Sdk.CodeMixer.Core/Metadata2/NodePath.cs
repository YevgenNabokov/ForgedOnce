﻿using System;
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

        public NodePath Concat(NodePath other)
        {
            if (other.Language != this.Language)
            {
                throw new ArgumentException($"Provided path should have the same {nameof(Language)}.", nameof(other));
            }

            return new NodePath(this.Language, this.Levels.Concat(other.Levels));
        }

        public NodePath RelativeTo(NodePath other)
        {
            if (other.Language != this.Language)
            {
                throw new ArgumentException($"Provided path should have the same {nameof(Language)}.", nameof(other));
            }

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
            if (other.Language != this.Language)
            {
                throw new ArgumentException($"Provided path should have the same {nameof(Language)}.", nameof(other));
            }

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
    }
}
