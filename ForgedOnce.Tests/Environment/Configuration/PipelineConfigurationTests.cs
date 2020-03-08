﻿using ForgedOnce.Environment.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Tests.Environment.Configuration
{
    [TestFixture]
    public class PipelineConfigurationTests
    {
        private readonly string TestConfigPayload = @"
{
    codeFileHandlers: 
    [
        { type: 'HandlerType1', config: {} },
        { type: 'HandlerType2' },
    ]
}
";

        [Test]
        public void CodeFileHandlersCanBeParsed()
        {
            var subject = new PipelineConfiguration(JObject.Parse(this.TestConfigPayload));

            var handlers = subject.CodeFileHandlerTypeRegistrations.ToList();

            Assert.AreEqual(2, handlers.Count);

            Assert.AreEqual("HandlerType1", handlers[0].Type);
            Assert.IsNotNull(handlers[0].Configuration);

            Assert.AreEqual("HandlerType2", handlers[1].Type);
            Assert.IsNull(handlers[1].Configuration);
        }
    }
}