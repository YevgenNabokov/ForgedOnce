using FluentAssertions;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Pipeline;
using Game08.Sdk.CodeMixer.Environment.Builders;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;
using Game08.Sdk.CodeMixer.Environment.Workspace.TypeLoaders;
using Game08.Sdk.CodeMixer.UnitTests.TestObjectFactories;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.UnitTests.Environment.Integration
{
    [TestFixture]
    public class PipelineConstructionTests
    {
        [Test]
        public void CanConstructPipeine()
        {
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict).Object;

            var basePath = "z:\\fakepath";

            var workspaceManager = new WorkspaceManager(TestWorkspaceFactory.GetWorkspace());
            BuilderRegistry builders = new BuilderRegistry();
            builders.Register<ICodeFileLocationProvider>(new WorkspaceCodeFileLocationProviderBuilder(workspaceManager));
            builders.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            var config = JObject.Parse(Resources.IntegrationTestConfig01);

            var subject = new PipelineBuilder(builders, workspaceManager, workspaceManager, basePath, fileSystem, new DefaultTypeLoader());

            var result = subject.Build(config);

            result.Should().NotBeNull();

            var pipeline = result as CodeGenerationPipeline;
        }
    }
}
