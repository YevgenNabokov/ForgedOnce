﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Configuration
{
    public class ConstructableTypeRegistration
    {
        public const string TypeKey = "type";

        public const string ConfigurationKey = "config";

        public const string NugetPackageIdKey = "nugetPackageId";

        public const string NugetPackageVersionKey = "nugetPackageVersion";

        private readonly JObject config;

        public ConstructableTypeRegistration(JObject config)
        {
            this.config = config;
        }

        public string Type
        {
            get
            {
                if (!this.config.ContainsKey(TypeKey))
                {
                    throw new InvalidOperationException($"Type registration {config.Path} does not contain {TypeKey}.");
                }

                return this.config[TypeKey].Value<string>();
            }
        }

        public JObject Configuration
        {
            get
            {
                if (this.config.ContainsKey(ConfigurationKey))
                {
                    return this.config[ConfigurationKey].Value<JObject>();
                }

                return null;
            }
        }

        public string NugetPackageId
        {
            get
            {
                if (this.config.ContainsKey(NugetPackageIdKey))
                {
                    return this.config[NugetPackageIdKey].Value<string>();
                }

                return null;
            }
        }

        public string NugetPackageVersion
        {
            get
            {
                if (this.config.ContainsKey(NugetPackageVersionKey))
                {
                    return this.config[NugetPackageVersionKey].Value<string>();
                }

                return null;
            }
        }
    }
}
