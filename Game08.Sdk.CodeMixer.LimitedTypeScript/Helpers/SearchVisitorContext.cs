﻿using Game08.Sdk.LTS.Model.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers
{
    public class SearchVisitorContext
    {
        public Type SearchedType;

        public List<Node> Result = new List<Node>();
    }
}
