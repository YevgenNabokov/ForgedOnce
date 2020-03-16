using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class BatchShadowFilter
    {
        public string Language;

        public ICodeFileLocationFilter Filter;
    }
}
