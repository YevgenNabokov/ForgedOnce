using FluentAssertions;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using LTSModel = Game08.Sdk.LTS.Model.DefinitionTree;
using Game08.Sdk.LTS.Model.TypeData;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.UnitTests.Integration
{
    [TestFixture]
    public class CodeFileEnvironmentHandlerLtsTests
    {
        [Test]
        public void CanTransformOutputs()
        {
            var fileSystem = new FileSystem();
            var path = fileSystem.Path.GetTempPath();
            var tempDir = fileSystem.Directory.CreateDirectory(fileSystem.Path.Combine(path, Guid.NewGuid().ToString()));
            var fileSystemMock = this.SetupRestrictedFileSystem(tempDir.FullName);

            CodeFileEnvironmentHandlerLts subject = new CodeFileEnvironmentHandlerLts(fileSystemMock);

            var codeFile = subject.CreateCodeFile("Test.ts") as CodeFileLtsModel;
            codeFile.Location = new Core.CodeFileLocation()
            {
                FilePath = "Z:\\TestFolder\\TestSubfolder\\Test.ts"
            };

            string className = "TestClass";
            var classKey = codeFile.TypeRepository.RegisterTypeDefinition(
                className,
                string.Empty,
                "SomeFilePath",
                Enumerable.Empty<TypeParameter>());

            var numberRef = codeFile.TypeRepository.RegisterTypeReferenceBuiltin("number");

            codeFile.Model = new FileRoot();
            var classDef = new ClassDefinition()
            {
                TypeKey = classKey,
                Name = new Identifier() { Name = className }
            };
            var fieldDec = new FieldDeclaration()
            {
                TypeReference = new TypeReferenceId() { ReferenceKey = numberRef },
                Name = new Identifier() { Name = "A" }
            };
            fieldDec.Modifiers.Add(LTSModel.Modifier.Public);

            classDef.Fields.Add(fieldDec);

            codeFile.Model.Items.Add(classDef);

            subject.Add(codeFile);
            subject.AddOutput(codeFile);
            subject.RefreshAndRecompile();

            var result = subject.GetOutputs().ToArray();

            result.Should().NotBeNullOrEmpty();
            result[0].Name.Should().Be(codeFile.Name);
            result[0].SourceCodeText.Should().NotBeNullOrEmpty();
            result[0].SourceCodeText.Should().Contain(className);

            fileSystem.Directory.Delete(tempDir.FullName, true);
        }

        private IFileSystem SetupRestrictedFileSystem(string rootFolder)
        {
            var result = new Mock<IFileSystem>(MockBehavior.Strict);
            var pathMock = new Mock<IPath>(MockBehavior.Strict);
            pathMock.Setup(m => m.IsPathRooted(It.IsAny<string>())).Returns<string>(p => System.IO.Path.IsPathRooted(p));
            pathMock.Setup(m => m.GetPathRoot(It.IsAny<string>())).Returns<string>(p => System.IO.Path.GetPathRoot(p));
            pathMock.Setup(m => m.GetTempPath()).Returns(rootFolder);
            pathMock.Setup(m => m.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((p1, p2) => System.IO.Path.Combine(p1, p2));

            var fileMock = new Mock<IFile>(MockBehavior.Strict);
            fileMock.Setup(m => m.ReadAllText(It.Is<string>(s => s.StartsWith(rootFolder)))).Returns<string>(p => System.IO.File.ReadAllText(p));

            result.Setup(m => m.Path).Returns(pathMock.Object);
            result.Setup(m => m.File).Returns(fileMock.Object);
            return result.Object;
        }
    }
}
