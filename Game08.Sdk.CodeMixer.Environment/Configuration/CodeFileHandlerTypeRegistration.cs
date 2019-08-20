using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class CodeFileHandlerTypeRegistration
    {
        public const string TypeKey = "Type";

        public const string ConfigurationKey = "Config";

        private readonly JObject config;

        public CodeFileHandlerTypeRegistration(JObject config)
        {
            this.config = config;
        }

        public string Type
        {
            get
            {
                if (!this.config.ContainsKey(TypeKey))
                {
                    throw new InvalidOperationException($"Handler configuration {config.Path} does not contain {TypeKey}.");
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
