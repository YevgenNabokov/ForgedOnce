using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjects.SimplePlugin
{
    public class SerializableAttributeAdder : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            //// OF COURSE there must be a verification that there is no such attribute already.
            var changedClass = node.AddAttributeLists(
        SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Serializable")))));
            return base.VisitClassDeclaration(changedClass);
        }
    }
}
