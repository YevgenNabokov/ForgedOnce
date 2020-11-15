using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Core.Metadata.Storage
{
    public class TagSelector
    {
        public TagSelector(IEnumerable<string> tagKeys, SearchCondition condition)
        {
            this.TagKeys = tagKeys != null ? tagKeys.ToList() : null;
            this.Condition = condition;
        }

        public IReadOnlyList<string> TagKeys { get; private set; }

        public SearchCondition Condition { get; private set; }
    }
}
