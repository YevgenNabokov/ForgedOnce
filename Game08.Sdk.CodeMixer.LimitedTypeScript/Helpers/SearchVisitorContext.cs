using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers
{
    public class SearchVisitorContext
    {
        public Type SearchedType;

        public Func<Node, bool> Selector;

        public List<Node> Result = new List<Node>();
    }
}
