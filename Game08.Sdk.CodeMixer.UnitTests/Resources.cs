﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests
{
    public class Resources
    {
        public static string IntegrationTestConfig01
        {
            get
            {
                return GetTestResource("Game08.Sdk.CodeMixer.UnitTests.Resources.PipelineConfigurations.IntegrationTestConfig01.json");
            }
        }

        private static string GetTestResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
