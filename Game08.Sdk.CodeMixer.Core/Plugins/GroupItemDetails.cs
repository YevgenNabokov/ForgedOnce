﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public class GroupItemDetails
    {
        public GroupItemDetails(IEnumerable<string> groupingTags = null)
        {
            if (groupingTags != null)
            {
                this.GroupingTags = new HashSet<string>(groupingTags);
            }
        }

        public HashSet<string> GroupingTags { get; } = new HashSet<string>();
    }
}
