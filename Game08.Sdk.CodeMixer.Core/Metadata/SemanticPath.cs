using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata
{
    public class SemanticPath
    {
        public SemanticPath(string language, IEnumerable<PathLevel> pathLevels)
        {
            this.Language = language;
            this.Parts = new List<PathLevel>(pathLevels);
        }

        public string Language { get; private set; }

        public IReadOnlyList<PathLevel> Parts { get; private set; }
    }
}
