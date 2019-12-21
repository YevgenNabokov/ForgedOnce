using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2
{
    public class NodePathLevel
    {
        public NodePathLevel(string name, int index)
        {
            this.Name = name;
            this.Index = index;
        }

        public string Name { get; private set; }

        public int Index { get; private set; }

        public override bool Equals(object obj)
        {
            var level = obj as NodePathLevel;
            if (level != null)
            {
                return level.Index == this.Index && level.Name == this.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;                
                hash = hash * 23 + this.Index.GetHashCode();
                hash = hash * 23 + (this.Name != null ? this.Name.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
