using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ForgedOnce.Tests.TestObjectFactories
{
    public class TestWorkspaceFactory
    {
        private static readonly Lazy<PortableExecutableReference> s_mscorlib = new Lazy<PortableExecutableReference>(
        () => AssemblyMetadata.CreateFromImage(TestResources.NetFX.v4_0_30319.mscorlib).GetReference(filePath: @"R:\v4_0_30319\mscorlib.dll"),
        LazyThreadSafetyMode.PublicationOnly);

        private static readonly Lazy<PortableExecutableReference> s_system = new Lazy<PortableExecutableReference>(
        () => AssemblyMetadata.CreateFromImage(TestResources.NetFX.v4_0_30319.System).GetReference(filePath: @"R:\v4_0_30319\System.dll", display: "System.dll"),
        LazyThreadSafetyMode.PublicationOnly);

        private static string sampleClassText = @"
using System;
namespace #NAMESPACE
    {
        public class #CLASS
        {
            public int A { get; set; }

            public string B { get; private set; }
        }
    }";

        public static Workspace GetWorkspace(IEnumerable<MetadataReference> additionalReferences = null, string ns = "MyNamespace", string className = "MyAwesomeClass", string projectName = "NewProject")
        {
            var workspace = new AdhocWorkspace();
            
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(
                projectId,
                versionStamp,
                projectName,
                projectName,
                LanguageNames.CSharp,
                metadataReferences: new MetadataReference[]
                {
                    s_mscorlib.Value,
                    s_system.Value,                    
                }.Concat(additionalReferences ?? Enumerable.Empty<MetadataReference>()),
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var newProject = workspace.AddProject(projectInfo);
            var sourceText = SourceText.From(sampleClassText.Replace("#NAMESPACE", ns).Replace("#CLASS", className), Encoding.Unicode);
            var newDocument = workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);

            return workspace;
        }

        public static byte[] BuildAndGetPe(Workspace workspace)
        {
            var compilation = workspace.CurrentSolution.Projects.First().GetCompilationAsync().Result;

            byte[] pe = null;
            using (var stream = new MemoryStream())
            {
                compilation.Emit(stream);
                pe = stream.GetBuffer();
            }

            return pe;
        }

        public static Workspace GetWorkspaceWithReference(string referredAssemblyPath, string referredNs, string referredClass, string referredAssemblyName)
        {
            var referredWorkspace = GetWorkspace(null, referredNs, referredClass, referredAssemblyName);            

            byte[] referredPe = BuildAndGetPe(referredWorkspace);
            
            var reference = AssemblyMetadata.CreateFromImage(referredPe).GetReference(filePath: referredAssemblyPath, display: $"{referredAssemblyName}.dll");

            return GetWorkspace(new MetadataReference[] { reference });
        }
    }
}
