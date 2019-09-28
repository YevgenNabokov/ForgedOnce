using Game08.Sdk.LTS.Builder;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers
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
