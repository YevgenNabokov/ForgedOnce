using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public class CodeFileLocation
    {
        public string FilePath;

        public virtual string GetFileName()
        {
            return Path.GetFileName(this.FilePath);
        }

        public override bool Equals(object obj)
        {
            var location = obj as CodeFileLocation;
            if (location == null)
            {
                return false;
            }

            return this.FilePath == location.FilePath;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (this.FilePath != null ? this.FilePath.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
