using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class PathLevel
    {
        public string Name;

        public string Type;

        public PathLevel(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
