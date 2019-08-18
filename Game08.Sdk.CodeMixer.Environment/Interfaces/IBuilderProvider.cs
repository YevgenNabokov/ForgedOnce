using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IBuilderProvider
    {
        IBuilder<TResult> Resolve<TResult>(string name, bool throwIfNotFound = true);
    }
}
