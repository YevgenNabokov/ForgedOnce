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

        public const string ShadowKey = "shadow";

        public const string UnshadowKey = "unshadow";

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

        public IEnumerable<CodeFileFilterRegistration> ShadowFilters
        {
            get
            {
                if (configuration.ContainsKey(ShadowKey))
                {
                    foreach (var f in configuration[ShadowKey].Value<JArray>())
                    {
                        yield return this.ParseCodeFileFilterRegistration(f);
                    }
                }
            }
        }

        public IEnumerable<CodeFileFilterRegistration> UnshadowFilters
        {
            get
            {
                if (configuration.ContainsKey(UnshadowKey))
                {
                    foreach (var f in configuration[UnshadowKey].Value<JArray>())
                    {
                        yield return this.ParseCodeFileFilterRegistration(f);
                    }
                }
            }
        }

        private CodeFileFilterRegistration ParseCodeFileFilterRegistration(JToken token)
        {
            var valueString = token.Value<string>();
            if (string.IsNullOrEmpty(valueString))
            {
                this.ThrowErrorForProviderConfiguration();
            }

            var parts = valueString.Split(':');
            if (parts.Length != 3)
            {
                this.ThrowErrorForProviderConfiguration();
            }

            var language = parts[0];
            CodeFileFilterType type;
            if (!Enum.TryParse(parts[1], out type))
            {
                this.ThrowErrorForProviderConfiguration();
            }

            var paths = parts[2];
            if (string.IsNullOrEmpty(paths))
            {
                this.ThrowErrorForProviderConfiguration();
            }

            return new CodeFileFilterRegistration()
            {
                Language = language,
                Type = type,
                Paths = paths.Split(';')
            };
        }

        private void ThrowErrorForProviderConfiguration()
        {
            throw new InvalidOperationException($"CodeFile shadowing filter configuration should be {this.GetTemplateDescription()}");
        }

        private string GetTemplateDescription()
        {
            return $"['<Language>:<Type>:<Paths>',..] where Type={String.Join("|", Enum.GetValues(typeof(CodeFileFilterType)))} and Paths is semicolon delimitered file system or project path masks.";
        }
    }
}
