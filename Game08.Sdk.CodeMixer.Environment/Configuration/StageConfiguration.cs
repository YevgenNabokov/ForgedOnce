using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class StageConfiguration
    {
        public const string InputSelectorKey = "input";

        public const string OutputsSelectorKey = "output";

        public const string CodeStreamMappingsKey = "mappings";

        public const string PluginKey = "plugin";

        public const string NameKey = "name";

        private readonly JObject configuration;

        public StageConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public BuilderConfiguration Input
        {
            get
            {
                if (this.configuration.ContainsKey(InputSelectorKey))
                {
                    return new BuilderConfiguration(this.configuration[InputSelectorKey].Value<JObject>());
                }

                throw new InvalidOperationException($"Stage configuration should contain {InputSelectorKey}.");
            }
        }

        public BuilderConfiguration Output
        {
            get
            {
                if (this.configuration.ContainsKey(OutputsSelectorKey))
                {
                    return new BuilderConfiguration(this.configuration[OutputsSelectorKey].Value<JObject>());
                }

                return null;
            }
        }

        public Dictionary<string, BuilderConfiguration> CodeStreamMappers
        {
            get
            {
                if (this.configuration.ContainsKey(CodeStreamMappingsKey))
                {
                    Dictionary<string, BuilderConfiguration> result = new Dictionary<string, BuilderConfiguration>();

                    foreach (var mapper in this.configuration[CodeStreamMappingsKey].Value<JObject>())
                    {
                        result.Add(mapper.Key, new BuilderConfiguration(mapper.Value.Value<JObject>()));
                    }

                    return result;
                }

                throw new InvalidOperationException($"Stage configuration should contain {CodeStreamMappingsKey}.");
            }
        }

        public PluginConfiguration Plugin
        {
            get
            {
                if (this.configuration.ContainsKey(PluginKey))
                {
                    return new PluginConfiguration(this.configuration[PluginKey].Value<JObject>());
                }

                throw new InvalidOperationException($"Stage configuration should contain {PluginKey}.");
            }
        }

        public string Name
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
    }
}
