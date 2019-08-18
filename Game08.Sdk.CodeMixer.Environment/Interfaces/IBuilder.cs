using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IBuilder<TResult>
    {
        string Name { get; }

        TResult Build(JObject configuration);
    }
}
