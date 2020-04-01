using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class Settings
    {
        public const string PropertyNamesKey = "propertyNames";

        public const string SkipPropertyNameIfConflictsKey = "skipPropertyNameIfConflicts";

        public string[] PropertyNames = new string[0];

        public bool SkipPropertyNameIfConflicts;

        public static Settings Read(JObject configuration)
        {
            var result = new Settings();

            if (configuration == null)
            {
                return result;
            }

            if (configuration.ContainsKey(PropertyNamesKey))
            {
                result.PropertyNames = configuration[PropertyNamesKey].Value<string>().Split(',', ';');
            }

            if (configuration.ContainsKey(SkipPropertyNameIfConflictsKey))
            {
                result.SkipPropertyNameIfConflicts = configuration[SkipPropertyNameIfConflictsKey].Value<bool>();
            }

            return result;
        }
    }
}
