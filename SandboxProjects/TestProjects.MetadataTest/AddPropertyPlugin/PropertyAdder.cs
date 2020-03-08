using ForgedOnce.Core.Metadata.Interfaces;
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
        private Dictionary<string, string> propertiesToAdd;

        public PropertyAdder(Dictionary<string, string> propertiesToAdd)
        {
            this.AnnotationKey = $"PropertyAdder_{Guid.NewGuid()}";
            this.propertiesToAdd = propertiesToAdd;
        }

        public string AnnotationKey
        {
            get;
            private set;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            List<PropertyDeclarationSyntax> newDeclarations = new List<PropertyDeclarationSyntax>();
            foreach (var prop in this.propertiesToAdd)
            {
                var newDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(prop.Value), prop.Key)
                    .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(new AccessorDeclarationSyntax[] {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    })))
                    .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

                newDeclaration = newDeclaration.WithAdditionalAnnotations(new SyntaxAnnotation(this.AnnotationKey));
                newDeclarations.Add(newDeclaration);
            }

            var changedClass = node.AddMembers(newDeclarations.ToArray());

            return base.VisitClassDeclaration(changedClass);
        }
    }
}
