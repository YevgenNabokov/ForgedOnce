using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class InputProviderConfiguration
    {
        private readonly JObject configuration;

        /// <summary>
        /// Configuration template: <ProviderName>:"<Language>:<Type>:<Paths>"
        /// </summary>
        /// <param name="configuration"></param>
        public InputProviderConfiguration(JObject configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<CodeStreamProviderRegistration> ProviderRegistrations
        {
            get
            {
                foreach (var key in this.configuration.Properties())
                {
                    var name = key.Name;

                    var valueString = key.Value.Value<string>();
                    if (string.IsNullOrEmpty(valueString))
                    {
                        this.ThrowErrorForProviderConfiguration(name);
                    }

                    var parts = valueString.Split(':');
                    if (parts.Length != 3)
                    {
                        this.ThrowErrorForProviderConfiguration(name);
                    }

                    var language = parts[0];
                    CodeStreamProviderType type;
                    if (!Enum.TryParse(parts[1], out type))
                    {
                        this.ThrowErrorForProviderConfiguration(name);
                    }
                    
                    var paths = parts[2];
                    if (string.IsNullOrEmpty(paths))
                    {
                        this.ThrowErrorForProviderConfiguration(name);
                    }

                    yield return new CodeStreamProviderRegistration()
                    {
                        Name = name,
                        Language = language,
                        Type = type,
                        Paths = paths.Split(';')
                    };
                }
            }
        }

        private void ThrowErrorForProviderConfiguration(string name)
        {
            throw new InvalidOperationException($"Code stream provider configuration {name} should be {this.GetTemplateDescription()}");
        }

        private string GetTemplateDescription()
        {
            return $"<ProviderName> : '<Language>:<Type>:<Paths>' where Type={String.Join("|", Enum.GetValues(typeof(CodeStreamProviderType)))} and Paths is semicolon delimitered file system or project path masks.";
        }
    }
}
