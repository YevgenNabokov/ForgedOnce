using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class PathLevel
    {        
        public PathLevel(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public override bool Equals(object obj)
        {
            var level = obj as PathLevel;
            if (level != null)
            {
                return level.Type == this.Type && level.Name == this.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (this.Type != null ? this.Type.GetHashCode() : 0);
                hash = hash * 23 + (this.Name != null ? this.Name.GetHashCode() : 0);                
                return hash;
            }
        }
    }
}
