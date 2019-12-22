using FluentAssertions;
using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.CSharp.Metadata2;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.UnitTests.Metadata
{
    [TestFixture]
    public class CSharpNodePathServiceTests
    {
        private string cSharpSource01 = @"
            namespace MyNamespace
            {
                public class MyHelperClass<T> 
                {
                    public T A;
                }

                public class MyAwesomeClass
                {
                    private int a;

                    public int B = 5;

                    public MyHelperClass<int> C;

                    public string D = ""Some value."";

                    public string PA { get; set; }

                    public int PB 
                    {
                        get
                        {
                            return this.B;
                        }

                        set
                        {
                            this.B = value;
                        }
                    }

                    public MyAwesomeClass()
                    {
                        this.PA = ""Some value."";
                    }

                    public int Add(int a, int b)
                    {
                        this.B = b;
                        var n = a + 5;
                        return n + b;
                    }

                    public int AddAndMultiply(int a, int b, int multiplier)
                    {
                        return this.Add(a, b) * multiplier;
                    }
                }
            }";

        [Test]
        public void CanGetPath()
        {
            var fileId = Guid.NewGuid().ToString();
            var codeFile = this.PrepareCodeFile(fileId, this.cSharpSource01);
            var subject = new CSharpNodePathService(codeFile);

            var node = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "MyAwesomeClass");

            var result = subject.GetNodePath(node);

            result.Should().NotBeNull();
            result.ToString().Should().Be($"{Languages.CSharp}:{fileId}/Members[0]/Members[1]");
        }

        [Test]
        public void CanGetNode()
        {
            var fileId = Guid.NewGuid().ToString();
            var codeFile = this.PrepareCodeFile(fileId, this.cSharpSource01);
            var subject = new CSharpNodePathService(codeFile);

            var node = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "MyAwesomeClass");

            var path = NodePath.FromString($"{Languages.CSharp}:{fileId}/Members[0]/Members[1]");
            var result = subject.ResolveNode(path);

            result.Should().Be(node);
        }

        private CodeFileCSharp PrepareCodeFile(string id, string sourceCode)
        {
            var result = new CodeFileCSharp(id, "Test.cs");
            result.SyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            return result;
        }
    }
}
