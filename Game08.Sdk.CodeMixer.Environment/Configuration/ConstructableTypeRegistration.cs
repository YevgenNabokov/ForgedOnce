using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class ConstructableTypeRegistration
    {
        public const string TypeKey = "type";

        public const string ConfigurationKey = "config";

        private readonly JObject config;

        public ConstructableTypeRegistration(JObject config)
        {
            this.config = config;
        }

        public string Type
        {
            get
            {
                if (!this.config.ContainsKey(TypeKey))
                {
                    throw new InvalidOperationException($"Type registration {config.Path} does not contain {TypeKey}.");
                }

                return this.config[TypeKey].Value<string>();
            }
        }

        public JObject Configuration
        {
            get
            {
                if (this.config.ContainsKey(ConfigurationKey))
                {
                    return this.config[ConfigurationKey].Value<JObject>();
                }

                return null;
            }
        }
    }
}
