using ForgedOnce.TsLanguageServices.ModelBuilder;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript.Helpers
{
    public class UpdateTypeReferencesVisitor : BuilderDefinitionTreeVisitor<Dictionary<string, string>>
    {
        public override void VisitTypeReferenceId(TypeReferenceId node, Dictionary<string, string> context)
        {
            if (context.ContainsKey(node.ReferenceKey))
            {
                node.ReferenceKey = context[node.ReferenceKey];
            }

            base.VisitTypeReferenceId(node, context);
        }
    }
}
