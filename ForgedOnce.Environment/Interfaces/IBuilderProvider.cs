using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IBuilderProvider
    {
        IBuilder<TResult> Resolve<TResult>(string name, bool throwIfNotFound = true);
    }
}
