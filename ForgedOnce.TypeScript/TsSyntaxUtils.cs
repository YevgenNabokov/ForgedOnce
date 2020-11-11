using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.TransportModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public static class TsSyntaxUtils
    {
        public static void CloneContent(CodeFileTs source, CodeFileTs target, IMetadataRecorder metadataRecorder)
        {
            if (source.Model != null)
            {
                var transport = (Node)source.Model.GetTransportModelNode();

                ModelConverter converter = new ModelConverter();

                target.Model = (StRoot)converter.ConvertFromNode(transport);
            }
        }
    }
}
