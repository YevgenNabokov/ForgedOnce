using Game08.Sdk.CodeMixer.Core.Metadata2;
using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.Metadata2
{
    public class SyntaxTreeScope : ISubTreeScope
    {
        private readonly CodeFileCSharp codeFileCSharp;

        private readonly PathHelperVisitor pathHelperVisitor;

        public SyntaxTreeScope(CodeFileCSharp codeFileCSharp, PathHelperVisitor pathHelperVisitor)
        {
            this.codeFileCSharp = codeFileCSharp;
            this.pathHelperVisitor = pathHelperVisitor;
        }

        public string AnnotationId => throw new NotImplementedException();

        public MetadataRoot[] ResolveRoots()
        {
            throw new NotImplementedException();
        }
    }
}
