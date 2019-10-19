using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDisplayNameAttrbutePlugin
{
    public class AttributeAdder : CSharpSyntaxRewriter
    {
        private readonly IEnumerable<PropertyDeclarationSyntax> propsToDecorate;

        public AttributeAdder(IEnumerable<PropertyDeclarationSyntax> propsToDecorate)
        {
            this.propsToDecorate = propsToDecorate;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (this.propsToDecorate.Contains(node))
            {
                var name = node.Identifier.ValueText;

                //// OF COURSE there must be a verification that there is no such attribute already.
                var changedProp = node.AddAttributeLists(
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                SyntaxFactory.Attribute(
                    SyntaxFactory.IdentifierName("DisplayName"),
                    SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal($"{name}_Decorated")))))))));
                return base.VisitPropertyDeclaration(changedProp);
            }
            
            return base.VisitPropertyDeclaration(node);
        }
    }
}
