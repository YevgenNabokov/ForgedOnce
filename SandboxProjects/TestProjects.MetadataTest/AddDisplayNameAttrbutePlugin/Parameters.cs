﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class Parameters
    {
        public IEnumerable<PropertyDeclarationSyntax> PropertiesToDecorate { get; set; }
    }
}
