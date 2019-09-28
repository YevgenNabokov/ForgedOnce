using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Modified : RecordBase
    {
        public Modified(ISemanticSymbol target)
        {
            this.Names.Add(target);
            this.Target = target;
        }

        public ISemanticSymbol Target { get; private set; }
    }
}
