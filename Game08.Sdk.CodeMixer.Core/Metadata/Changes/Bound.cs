using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Bound : RecordBase
    {
        public Bound(ISemanticSymbol item1, ISemanticSymbol item2)
        {
            this.Names.Add(item1);
            this.Names.Add(item2);
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public ISemanticSymbol Item1
        {
            get;

            private set;
        }

        public ISemanticSymbol Item2
        {
            get;

            private set;
        }
    }
}
