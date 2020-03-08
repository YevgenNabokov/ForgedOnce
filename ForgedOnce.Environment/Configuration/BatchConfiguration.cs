using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class BatchConfiguration
    {
        public const string NameKey = "name";

        public const string StagesKey = "stages";

        public const string PersistCodeInputStreamsKey = "persistInputCodeStreams";

        private readonly JObject configuration;

        public BatchConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public string Name
        {
            get
            {
                if (configuration.ContainsKey(NameKey))
                {
                    return configuration[NameKey].Value<string>();
                }

                return null;
            }
        }

        public string[] PersistCodeInputStreams
        {
            get
            {
                if (configuration.ContainsKey(PersistCodeInputStreamsKey))
                {
                    return configuration[PersistCodeInputStreamsKey].Values<string>().ToArray();
                }

                return Array.Empty<string>();
            }
        }

        public IEnumerable<JObject> Stages
        {
            get
            {
                if (configuration.ContainsKey(StagesKey))
                {
                    foreach (var stage in configuration[StagesKey].AsJEnumerable())
                    {
                        yield return stage.Value<JObject>();
                    }
                }
            }
        }
    }
}
