using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class NodePathLevel
    {
        public NodePathLevel(string name, int? index)
        {
            this.Name = name;
            this.Index = index;
        }

        public string Name { get; private set; }

        public int? Index { get; private set; }

        public override string ToString()
        {
            return this.Index.HasValue ? $"{this.Name}[{this.Index}]" : this.Name;
        }

        public static NodePathLevel FromString(string level)
        {
            if (level.Contains("["))
            {
                var openBracketIndex = level.IndexOf("[");
                return new NodePathLevel(level.Substring(0, openBracketIndex), Int32.Parse(level.Substring(openBracketIndex + 1, level.IndexOf("]") - openBracketIndex - 1)));
            }
            else
            {
                return new NodePathLevel(level, null);
            }
        }

        public override bool Equals(object obj)
        {
            var level = obj as NodePathLevel;
            if (level != null)
            {
                return ((!level.Index.HasValue && !this.Index.HasValue) || (level.Index == this.Index))
                    && ((level.Name is null && this.Name is null) || level.Name == this.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;                
                hash = hash * 23 + (this.Index != null ? this.Index.GetHashCode() : 0);
                hash = hash * 23 + (this.Name != null ? this.Name.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
