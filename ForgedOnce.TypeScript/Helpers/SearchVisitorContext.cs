using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript.Helpers
{
    public class SearchVisitorContext
    {
        public Type SearchedType;

        public Func<StNodeBase, bool> Selector;

        public List<StNodeBase> Result = new List<StNodeBase>();
    }
}
