using ForgedOnce.Core.Metadata.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.CSharp
{
    public static class CSharpSyntaxUtils
    {
        public static void CloneContent(CodeFileCSharp source, CodeFileCSharp target, IMetadataRecorder metadataRecorder, Dictionary<string, string> metadataTags = null, object metadataObject = null)
        {
            if (source.SyntaxTree != null)
            {
                var sourceRoot = (CSharpSyntaxNode)source.SyntaxTree.GetRoot();
                var targetTree = CSharpSyntaxTree.Create(sourceRoot);
                var targetRoot = targetTree.GetRoot();
                target.SyntaxTree = targetTree;
                metadataRecorder.SymbolSourcingFrom(source.NodePathService, sourceRoot, target.NodePathService, targetRoot, metadataTags ?? new Dictionary<string, string>(), metadataObject);
            }
        }
    }
}
