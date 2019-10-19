using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class PropertyAdder : CSharpSyntaxRewriter
    {
        public PropertyAdder()
        {
            this.AddedProperties = new List<PropertyDeclarationSyntax>();
        }

        public List<PropertyDeclarationSyntax> AddedProperties
        {
            get; private set;
        }        

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var newDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), "AddedProp")
                .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(new AccessorDeclarationSyntax[] {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                })))
                .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

            this.AddedProperties.Add(newDeclaration);

            var changedClass = node.AddMembers(newDeclaration);

            return base.VisitClassDeclaration(changedClass);
        }
    }
}
