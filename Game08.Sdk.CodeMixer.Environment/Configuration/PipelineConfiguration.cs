using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class PipelineConfiguration
    {
        public const string CodeFileHandlerTypeRegistrationsKey = "codeFileHandlers";

        public const string InputCodeStreamsKey = "inputs";

        public const string BatchesKey = "batches";

        private JObject configuration;

        public PipelineConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<ConstructableTypeRegistration> CodeFileHandlerTypeRegistrations
        {
            get
            {
                if (configuration.ContainsKey(CodeFileHandlerTypeRegistrationsKey))
                {
                    foreach (var handler in configuration[CodeFileHandlerTypeRegistrationsKey].AsJEnumerable())
                    {
                        yield return new ConstructableTypeRegistration(handler.Value<JObject>());
                    }
                }
            }
        }

        public IEnumerable<BatchConfiguration> BatchConfigurations
        {
            get
            {
                if (configuration.ContainsKey(BatchesKey))
                {
                    foreach (var batch in configuration[BatchesKey].AsJEnumerable())
                    {
                        yield return new BatchConfiguration(batch.Value<JObject>());
                    }
                }
            }
        }

        public JObject InputCodeStreamProviderConfiguration
        {
            get
            {
                if (configuration.ContainsKey(InputCodeStreamsKey))
                {
                    return configuration[InputCodeStreamsKey].Value<JObject>();
                }

                return null;
            }
        }
    }
}
