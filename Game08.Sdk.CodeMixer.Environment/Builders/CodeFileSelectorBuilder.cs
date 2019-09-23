using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Configuration;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class CodeFileSelectorBuilder : IBuilder<ICodeFileSelector>
    {
        public string Name => null;

        public ICodeFileSelector Build(JObject configuration)
        {
            var reader = new CodeFileSelectorConfiguration(configuration);

            Dictionary<string, string[]> filters = new Dictionary<string, string[]>();

            foreach (var filter in reader.Filters)
            {
                filters.Add(filter.Key, filter.Value.Split(';'));
            }

            return new CodeFileSelector(filters);
        }
    }
}
