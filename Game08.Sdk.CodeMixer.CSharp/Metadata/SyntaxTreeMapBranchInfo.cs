using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata
{
    public class SyntaxTreeMapBranchInfo
    {
        public SyntaxTreeMapBranchInfo(PropertyInfo property, Type itemType, bool isToken, bool isCollection)
        {
            this.Property = property;
            this.IsCollection = isCollection;
            this.IsToken = isToken;
            this.ItemType = itemType;
        }

        public PropertyInfo Property { get; private set; }

        public Type ItemType { get; private set; }

        public bool IsCollection { get; private set; }

        public bool IsToken { get; private set; }
    }
}
