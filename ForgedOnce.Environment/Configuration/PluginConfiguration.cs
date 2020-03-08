using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class PluginConfiguration
    {
        public const string PreprocessorKey = "preprocessor";

        public const string PluginKey = "pluginFactory";

        private readonly JObject configuration;

        public PluginConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public ConstructableTypeRegistration Preprocessor
        {
            get
            {
                if (this.configuration.ContainsKey(PreprocessorKey))
                {
                    return new ConstructableTypeRegistration(this.configuration[PreprocessorKey].Value<JObject>());
                }

                return null;
            }
        }

        public ConstructableTypeRegistration PluginFactory
        {
            get
            {
                if (this.configuration.ContainsKey(PluginKey))
                {
                    return new ConstructableTypeRegistration(this.configuration[PluginKey].Value<JObject>());
                }

                throw new InvalidOperationException($"Plugin configuration should have {PluginKey}.");
            }
        }
    }
}
