using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace.TypeLoaders;
using Game08.Sdk.CodeMixer.UnitTests.TestObjectFactories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests.Environment.Workspace
{
    [TestFixture]
    public class WorkspaceTypeLoaderTests
    {
        [Test]
        public void CanLoadTypeFromWorkspace()
        {
            var pluginNamespace = "TestPlugins";
            var pluginClass = "MyPlugin";
            var pluginAssembly = "MyPlugins";
            var pluginWorkspace = TestWorkspaceFactory.GetWorkspace(null, pluginNamespace, pluginClass, pluginAssembly);

            var subject = new WorkspaceTypeLoader(new WorkspaceManager(pluginWorkspace));

            var result = subject.LoadType($"{pluginNamespace}.{pluginClass}, {pluginAssembly}");

            Assert.IsNotNull(result);
            Assert.AreEqual(pluginClass, result.Name);
        }
    }
}
