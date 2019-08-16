using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Interfaces
{
    public interface IResolvableName : ISemanticName
    {
        void Resolve();
    }
}
