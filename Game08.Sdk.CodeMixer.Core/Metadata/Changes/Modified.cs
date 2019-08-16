using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Changes
{
    public class Modified : RecordBase
    {
        public Modified(ISemanticName target)
        {
            this.Names.Add(target);
            this.Target = target;
        }

        public ISemanticName Target { get; private set; }
    }
}
