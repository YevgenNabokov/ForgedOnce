using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class BuilderConfiguration
    {
        public const string NameKey = "name";

        public const string SettingsKey = "settings";

        private readonly JObject configuration;

        public BuilderConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public string BuilderName
        {
            get
            {
                if (this.configuration.ContainsKey(NameKey))
                {
                    return this.configuration[NameKey].Value<string>();
                }

                return null;
            }
        }

        public JObject Configuration
        {
            get
            {
                if (this.configuration.ContainsKey(SettingsKey))
                {
                    return this.configuration[SettingsKey].Value<JObject>();
                }

                throw new InvalidOperationException($"Builder reference should contain {NameKey} and {SettingsKey}.");
            }
        }
    }
}
