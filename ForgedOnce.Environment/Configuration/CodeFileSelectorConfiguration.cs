using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class CodeFileSelectorConfiguration
    {
        private readonly JObject configuration;

        public CodeFileSelectorConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public Dictionary<string, string> Filters
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                foreach (var prop in this.configuration.Properties())
                {
                    result.Add(prop.Name, prop.Value.Value<string>());
                }

                return result;
            }
        }
    }
}
