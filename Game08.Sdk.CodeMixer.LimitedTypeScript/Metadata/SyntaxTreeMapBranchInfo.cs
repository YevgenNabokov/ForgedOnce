using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata
{
    public class SyntaxTreeMapBranchInfo
    {
        public SyntaxTreeMapBranchInfo(PropertyInfo property, Type itemType, bool isCollection)
        {
            this.Property = property;
            this.IsCollection = isCollection;
            this.ItemType = itemType;
        }

        public PropertyInfo Property { get; private set; }

        public Type ItemType { get; private set; }

        public bool IsCollection { get; private set; }
    }
}
