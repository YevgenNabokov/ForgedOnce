using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp.MsBuild
{
    public class CSharpMsBuildStoreAdapter : DefaultItemStoreAdapter
    {
        public const string CompileItemName = "Compile";

        public CSharpMsBuildStoreAdapter(IFileSystem fileSystem)
            : base(fileSystem, CompileItemName)
        {
        }

        public override bool CodeFileSupported(CodeFile codeFile)
        {
            return codeFile is CodeFileCSharp;
        }
    }
}
