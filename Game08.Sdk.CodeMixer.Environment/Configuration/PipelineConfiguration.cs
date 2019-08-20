using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class PipelineConfiguration
    {
        public const string CodeFileHandlerTypeRegistrationsKey = "CodeFileHandlers";

        private JObject configuration;

        public PipelineConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<CodeFileHandlerTypeRegistration> CodeFileHandlerTypeRegistrations
        {
            get
            {
                if (configuration.ContainsKey(CodeFileHandlerTypeRegistrationsKey))
                {
                    foreach (var handler in configuration[CodeFileHandlerTypeRegistrationsKey].AsJEnumerable())
                    {
                        yield return new CodeFileHandlerTypeRegistration(handler.Value<JObject>());
                    }
                }
            }
        }
    }
}
