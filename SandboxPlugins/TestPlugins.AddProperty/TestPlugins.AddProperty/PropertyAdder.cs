using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class PropertyAdder : CSharpSyntaxRewriter
    {
        private readonly SemanticModel semanticModel;
        private readonly Parameters parameters;

        public PropertyAdder(SemanticModel semanticModel, Parameters parameters)
        {
            this.semanticModel = semanticModel;
            this.parameters = parameters;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var changedClass = node;
            var declaredSymbol = this.semanticModel.GetDeclaredSymbol(node);
            var typeName = declaredSymbol.GetFullMetadataName();
            if (parameters.TypePropertyNames.ContainsKey(typeName))
            {
                foreach (var name in parameters.TypePropertyNames[typeName])
                {
                    changedClass = changedClass.AddMembers(SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), name)
                    .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(new AccessorDeclarationSyntax[] {
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    })))
                    .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))));
                }
            }

            return changedClass;
        }
    }
}
