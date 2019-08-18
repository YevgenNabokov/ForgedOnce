using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Configuration
{
    public class PipelineConfiguration
    {
        private JObject configuration;

        public PipelineConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }


    }
}
