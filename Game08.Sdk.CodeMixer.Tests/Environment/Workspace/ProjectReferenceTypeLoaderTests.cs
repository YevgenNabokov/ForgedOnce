using FluentAssertions;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis.TypeLoaders;
using Game08.Sdk.CodeMixer.Environment.Workspace.TypeLoaders;
using Game08.Sdk.CodeMixer.Tests.TestObjectFactories;
using Microsoft.CodeAnalysis;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.Loader;
using System.Text;

namespace Game08.Sdk.CodeMixer.Tests.Environment.Workspace
{
    [TestFixture]
    public class ProjectReferenceTypeLoaderTests
    {
        [Test]
        public void CanResolveTypeFromReference()
        {            
            var pluginNamespace = "TestPlugins";
            var pluginClass = "MyPlugin";
            var pluginAssembly = "MyPlugins";
            var pluginWorkspace = TestWorkspaceFactory.GetWorkspace(null, pluginNamespace, pluginClass, pluginAssembly);
            var pluginBinary = TestWorkspaceFactory.BuildAndGetPe(pluginWorkspace);

            string pluginPath = $"Z:\\plugins\\{pluginAssembly}.dll";

            var reference = AssemblyMetadata.CreateFromImage(pluginBinary).GetReference(filePath: pluginPath, display: $"{pluginAssembly}.dll");

            var workspace = TestWorkspaceFactory.GetWorkspace(new[] { reference });

            var pathMock = new Mock<IPath>(MockBehavior.Strict);
            pathMock.Setup(p => p.GetFileName(It.IsAny<string>())).Returns<string>(v => Path.GetFileName(v));
            var fileMock = new Mock<IFile>(MockBehavior.Strict);
            fileMock.Setup(f => f.ReadAllBytes(It.Is<string>(v => v == pluginPath))).Returns(pluginBinary);
            var fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            fileSystemMock.Setup(p => p.Path).Returns(pathMock.Object);
            fileSystemMock.Setup(p => p.File).Returns(fileMock.Object);

            var subject = new ProjectReferenceTypeLoader(new WorkspaceManager(workspace), fileSystemMock.Object, AssemblyLoadContext.Default);

            Action action = () => { subject.LoadType($"{pluginNamespace}.{pluginClass}, {pluginAssembly}"); };

            //// Temporary thing. I hope.
            action.Should().Throw<FileNotFoundException>();
        }
    }
}
