using FluentAssertions;
using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.CSharp.Metadata2;
using Microsoft.CodeAnalysis;
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
    public class SingleNodeSnapshotTests
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

                    //// It is there because when it will be moved no other member sohuld change index for a clean test.
                    public string FieldToMove = ""I am moving."";
                }
            }";

        [Test]
        public void CanAnnotate()
        {
            var fileId = Guid.NewGuid().ToString();
            var codeFile = this.PrepareCodeFile(fileId, this.cSharpSource01);
            var subject = new SingleNodeSnapshot(codeFile, new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>());

            var node = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "MyAwesomeClass");

            var annotatedRoot = subject.Initialize(node);

            var annotatedNode = annotatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "MyAwesomeClass");

            var allAnnotatedNodes = annotatedNode.GetAnnotatedNodes(subject.GetOriginalPathAnnotationKey()).ToArray();
            var allAnnotatedInRoot = annotatedNode.SyntaxTree.GetRoot().GetAnnotatedNodes(subject.GetOriginalPathAnnotationKey()).ToArray();

            allAnnotatedNodes.Length.Should().Be(1);
            allAnnotatedInRoot.Length.Should().Be(1);
        }

        [Test]
        public void CanGetRootWhenUnmodified()
        {
            var fileId = Guid.NewGuid().ToString();
            var codeFile = this.PrepareCodeFile(fileId, this.cSharpSource01);
            var subject = new SingleNodeSnapshot(codeFile, new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>());

            var node = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "MyAwesomeClass");

            var annotatedRoot = subject.Initialize(node);

            codeFile.SyntaxTree = annotatedRoot.SyntaxTree;

            var result = subject.ResolveRoot();

            result.Should().NotBeNull();
            result.CurrentPath.ToString().Should().Be($"{Languages.CSharp}:{fileId}/Members[0]/Members[1]");
            result.OriginalPath.ToString().Should().Be($"{Languages.CSharp}:{fileId}/Members[0]/Members[1]");
        }

        [Test]
        public void CanGetRootsWhenNodeMoved()
        {
            var fileId = Guid.NewGuid().ToString();
            var codeFile = this.PrepareCodeFile(fileId, this.cSharpSource01);
            var subject = new SingleNodeSnapshot(codeFile, new SyntaxTreeMappedVisitor<SyntaxTreeMappedVisitorContext>());

            var node = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>().First(f => f.Declaration.Variables.Count > 0 && f.Declaration.Variables.First().Identifier.Text == "FieldToMove");

            var annotatedRoot = subject.Initialize(node);
            codeFile.SyntaxTree = annotatedRoot.SyntaxTree;

            var fieldToMove = codeFile.SyntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>().First(f => f.Declaration.Variables.Count > 0 && f.Declaration.Variables.First().Identifier.Text == "FieldToMove");

            FieldRemover remover = new FieldRemover("FieldToMove");
            codeFile.SyntaxTree = remover.Visit(codeFile.SyntaxTree.GetRoot()).SyntaxTree;

            FieldAdder adder = new FieldAdder("MyHelperClass", fieldToMove);
            codeFile.SyntaxTree = adder.Visit(codeFile.SyntaxTree.GetRoot()).SyntaxTree;

            var result = subject.ResolveRoot();

            result.Should().NotBeNull();
            result.CurrentPath.ToString().Should().Be($"{Languages.CSharp}:{fileId}/Members[0]/Members[0]/Members[1]");
            result.OriginalPath.ToString().Should().Be($"{Languages.CSharp}:{fileId}/Members[0]/Members[1]/Members[9]");
        }

        private CodeFileCSharp PrepareCodeFile(string id, string sourceCode)
        {
            var result = new CodeFileCSharp(id, "Test.cs");
            result.SyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            return result;
        }

        private class FieldRemover : CSharpSyntaxRewriter
        {
            private readonly string name;

            public FieldRemover(string name)
            {
                this.name = name;
            }

            public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
            {
                if (node.Declaration.Variables.Count > 0 && node.Declaration.Variables.First().Identifier.Text == name)
                {
                    return null;
                }

                return base.VisitFieldDeclaration(node);
            }
        }

        private class FieldAdder : CSharpSyntaxRewriter
        {
            private readonly string className;
            private readonly FieldDeclarationSyntax field;

            public FieldAdder(string className, FieldDeclarationSyntax field)
            {
                this.className = className;
                this.field = field;
            }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                if (node.Identifier.Text == this.className)
                {
                    return node.AddMembers(this.field);
                }

                return base.VisitClassDeclaration(node);
            }
        }
    }
}
