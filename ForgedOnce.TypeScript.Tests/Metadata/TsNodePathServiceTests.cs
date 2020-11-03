using FluentAssertions;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstBuilder;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TsLanguageServices.Host.Interfaces;
using ForgedOnce.TypeScript.Metadata;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript.Tests.Metadata
{
    [TestFixture]
    public class TsNodePathServiceTests
    {
        [Test]
        public void CanBuildNodePath()
        {
            var hostMock = new Mock<ITsHost>(MockBehavior.Loose).Object;

            var file = new CodeFileTs("TestId", "Test.ts", hostMock);

            var node = new StReturnStatement().WithExpression(new StStringLiteral().WithText("Result."));

            file.Model = new StRoot()
                .WithStatement(new StClassDeclaration()
                    .WithName(new StIdentifier().WithEscapedText("TestClass"))
                    .WithModifier(new StPublicKeywordToken())
                    .WithMember(new StMethodDeclaration()
                        .WithName(new StIdentifier().WithEscapedText("numMethod"))
                        .WithModifier(new StPublicKeywordToken())
                        .WithBody(b => b.WithStatement(new StReturnStatement().WithExpression(new StNumericLiteral().WithText("5")))))
                    .WithMember(new StMethodDeclaration()
                        .WithName(new StIdentifier().WithEscapedText("strMethod"))
                        .WithModifier(new StPublicKeywordToken())
                        .WithBody(b => b.WithStatement(node))));

            var subject = new TsNodePathService(file);

            var path = subject.GetNodePath(node);

            path.Should().NotBeNull();
            path.ToString().Should().Be("TypeScript:TestId/statements[0]/members[1]/body/statements[0]");
        }
    }
}
