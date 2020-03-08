using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript.Helpers
{
    public class SearchVisitorContext
    {
        public Type SearchedType;

        public Func<Node, bool> Selector;

        public List<Node> Result = new List<Node>();
    }
}
