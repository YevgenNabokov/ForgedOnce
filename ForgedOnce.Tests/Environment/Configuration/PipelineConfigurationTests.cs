using FluentAssertions;
using ForgedOnce.Environment.Configuration;
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

        private readonly string TestBatchConfigPayload = @"
{
      'name': 'TestBatch',
      'stages': [
        {
          'name': 'TestStage01',
          'plugin': {
            'pluginFactory': { 'type': 'Test.PluginFactory, Test' }
          },
          'input': {
            'settings': {
              'TestInput': '*.cs'
            }
          },
          'destinationMapping': {
            'TestOutput': {
              'settings': { 'path': 'Test/Generated' }
            }
          },
          'outputStreamRenames': {
            'TestOutput': 'TestOutputRenamed'
          },
          'cleanDestinations': true
        }
      ],
      'persistInputCodeStreams': [ 'Stream01', 'Stream01' ],
      'shadow': [ 'CSharp:Project:Test/Folder/*.cs' ],
      'unshadow': [ 'Glsl:FileSystem:Test\\Shaders\\*.glsl' ]
    }
";

        [Test]
        public void CodeFileHandlersCanBeParsed()
        {
            var subject = new PipelineConfiguration(JObject.Parse(this.TestConfigPayload));

            var handlers = subject.CodeFileHandlerTypeRegistrations.ToList();


            handlers.Count.Should().Be(2);
            handlers[0].Type.Should().Be("HandlerType1");
            handlers[0].Configuration.Should().NotBeNull();
            handlers[1].Type.Should().Be("HandlerType2");
            handlers[1].Configuration.Should().BeNull();
        }

        [Test]
        public void BatchConfigurationCanBeParsed()
        {
            var subject = new BatchConfiguration(JObject.Parse(this.TestBatchConfigPayload));

            subject.ShadowFilters.Should().NotBeNull();
            subject.ShadowFilters.Count().Should().Be(1);
            subject.ShadowFilters.First().Language.Should().Be("CSharp");
            subject.ShadowFilters.First().Type.Should().Be(CodeFileFilterType.Project);
            subject.ShadowFilters.First().Paths.Should().NotBeNull();
            subject.ShadowFilters.First().Paths.Length.Should().Be(1);
            subject.ShadowFilters.First().Paths[0].Should().Be("Test/Folder/*.cs");

            subject.UnshadowFilters.Should().NotBeNull();
            subject.UnshadowFilters.Count().Should().Be(1);
            subject.UnshadowFilters.First().Language.Should().Be("Glsl");
            subject.UnshadowFilters.First().Type.Should().Be(CodeFileFilterType.FileSystem);
            subject.UnshadowFilters.First().Paths.Should().NotBeNull();
            subject.UnshadowFilters.First().Paths.Length.Should().Be(1);
            subject.UnshadowFilters.First().Paths[0].Should().Be("Test\\Shaders\\*.glsl");
        }
    }
}
