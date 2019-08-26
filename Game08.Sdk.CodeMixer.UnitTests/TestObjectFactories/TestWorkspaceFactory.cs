using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests.TestObjectFactories
{
    public class TestWorkspaceFactory
    {
        private static string sampleClassText = @"
using System;
namespace MyNamespace
    {
        public class MyAwesomeClass
        {
            public int A { get; set; }

            public string B { get; private set; }
        }
    }";

        public static Workspace GetWorkspace()
        {
            var workspace = new AdhocWorkspace();

            string projName = "NewProject";
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, projName, projName, LanguageNames.CSharp);
            var newProject = workspace.AddProject(projectInfo);
            var sourceText = SourceText.From(sampleClassText);
            var newDocument = workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);

            return workspace;
        }
    }
}
