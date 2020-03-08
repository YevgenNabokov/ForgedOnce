using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IBuilder<TResult>
    {
        string Name { get; }

        TResult Build(JObject configuration);
    }
}
