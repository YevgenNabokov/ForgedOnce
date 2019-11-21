using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public class GroupItemDetailsParametrized<TInputParameters> : GroupItemDetails
    {
        public GroupItemDetailsParametrized(TInputParameters parameters, IEnumerable<string> groupingTags = null)
            :base(groupingTags)
        {
            this.Parameters = parameters;
        }

        public TInputParameters Parameters { get; }
    }
}
