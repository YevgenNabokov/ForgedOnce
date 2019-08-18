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
    }
}
