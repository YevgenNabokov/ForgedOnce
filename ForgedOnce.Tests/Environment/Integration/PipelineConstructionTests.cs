﻿using FluentAssertions;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Pipeline;
using ForgedOnce.Environment.Builders;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Workspace.CodeAnalysis;
using ForgedOnce.Environment.Workspace.CodeFileLocationFilters;
using ForgedOnce.Environment.Workspace.TypeLoaders;
using ForgedOnce.Tests.TestObjectFactories;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO.Abstractions;

namespace ForgedOnce.Tests.Environment.Integration
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
            PipelineWorkspaceManagersWrapper workspaces = new PipelineWorkspaceManagersWrapper(workspaceManager, workspaceManager);
            BuilderRegistry builders = new BuilderRegistry();
            builders.Register<ICodeFileDestination>(new WorkspaceCodeFileDestinationBuilder(workspaces));
            builders.Register<ICodeFileSelector>(new CodeFileSelectorBuilder());

            var config = JObject.Parse(Resources.IntegrationTestConfig01);

            var subject = new PipelineBuilder(builders, workspaces, basePath, fileSystem, new DefaultTypeLoader(), logger);

            var result = subject.Build(config);

            result.Should().NotBeNull();

            var pipeline = result as CodeGenerationPipeline;

            pipeline.InputCodeStreamProvider.Should().NotBeNull();
            pipeline.PipelineEnvironment.Should().NotBeNull();

            pipeline.Batches.Should().NotBeNull();
            pipeline.Batches.Count.Should().Be(1);

            pipeline.Batches[0].Shadow.Should().NotBeNull();
            pipeline.Batches[0].Shadow.Count.Should().Be(1);
            pipeline.Batches[0].Shadow[0].Language.Should().Be("CSharp");
            pipeline.Batches[0].Shadow[0].Filter.Should().NotBeNull();
            pipeline.Batches[0].Shadow[0].Filter.Should().BeOfType<WorkspaceCodeFileLocationFilter>();
        }
    }
}
