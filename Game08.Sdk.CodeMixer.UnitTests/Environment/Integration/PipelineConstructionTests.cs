using FluentAssertions;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Pipeline;
using Game08.Sdk.CodeMixer.Environment.Builders;
using Game08.Sdk.CodeMixer.Environment.Workspace;
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
            var logger = new Mock<ILogger>(MockBehavior.Loose).Object;

            var basePath = "z:\\fakepath";

            var workspaceManager = new WorkspaceManager(TestWorkspaceFactory.GetWorkspace());
            PipelineWorkspaceManagersWrapper workspaces = new PipelineWorkspaceManagersWrapper(workspaceManager, workspaceManager, workspaceManager);
            BuilderRegistry builders = new BuilderRegistry();
            builders.Register<ICodeFileDestination>(new WorkspaceCodeFileDestinationBuilder(workspaces));
            builders.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            var config = JObject.Parse(Resources.IntegrationTestConfig01);

            var subject = new PipelineBuilder(builders, workspaces, basePath, fileSystem, new DefaultTypeLoader(), logger);

            var result = subject.Build(config);

            result.Should().NotBeNull();

            var pipeline = result as CodeGenerationPipeline;
        }
    }
}
