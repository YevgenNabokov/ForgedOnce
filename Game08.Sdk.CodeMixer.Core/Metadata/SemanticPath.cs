using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class SemanticPath
    {
        public string Language;

        public IReadOnlyList<PathLevel> Parts { get; private set; }
    }
}
