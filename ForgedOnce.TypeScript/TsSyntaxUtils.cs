using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.ModelBuilder.SyntaxTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public static class TsSyntaxUtils
    {
        public static void CloneContent(CodeFileTsModel source, CodeFileTsModel target, IMetadataRecorder metadataRecorder)
        {
            if (source.Model != null)
            {
                CloningDefinitionTreeVisitor cloner = new CloningDefinitionTreeVisitor();
                target.Model = cloner.CloneNode(source.Model);
            }
        }
    }
}
