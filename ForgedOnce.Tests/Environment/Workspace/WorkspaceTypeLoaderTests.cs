using ForgedOnce.Environment.Workspace.CodeAnalysis;
using ForgedOnce.Environment.Workspace.CodeAnalysis.TypeLoaders;
using ForgedOnce.Tests.TestObjectFactories;
using NUnit.Framework;
using System.Runtime.Loader;

namespace ForgedOnce.Tests.Environment.Workspace
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

            var subject = new WorkspaceTypeLoader(new WorkspaceManager(pluginWorkspace), AssemblyLoadContext.Default);

            var result = subject.LoadType($"{pluginNamespace}.{pluginClass}, {pluginAssembly}");

            Assert.IsNotNull(result);
            Assert.AreEqual(pluginClass, result.Name);
        }
    }
}
