using ForgedOnce.Core;
using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class ShadowFilter : ICodeFileLocationFilter
    {
        protected readonly List<SubFilter> Filters = new List<SubFilter>();

        public bool IsMatch(CodeFileLocation location)
        {
            var result = false;
            foreach (var filter in this.Filters)
            {
                if (filter.Filter.IsMatch(location))
                {
                    result = filter.Operation == Operation.Shadow;
                }
            }

            return result;
        }

        public void Shadow(ICodeFileLocationFilter filter)
        {
            this.Filters.Add(new SubFilter()
            {
                Filter = filter,
                Operation = Operation.Shadow
            });
        }

        public void Unshadow(ICodeFileLocationFilter filter)
        {
            this.Filters.Add(new SubFilter()
            {
                Filter = filter,
                Operation = Operation.Unshadow
            });
        }

        protected class SubFilter
        {
            public ICodeFileLocationFilter Filter;

            public Operation Operation;
        }

        protected enum Operation
        {
            Shadow,
            Unshadow
        }
    }
}
