using ForgedOnce.Core;
using ForgedOnce.Launcher.MSBuild.Storage;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.TypeScript.MsBuild
{
    public class TypeScriptMsBuildStoreAdapter : DefaultItemStoreAdapter
    {
        public const string TypeScriptCompileItemName = "TypeScriptCompile";

        public TypeScriptMsBuildStoreAdapter(IFileSystem fileSystem)
            :base(fileSystem, TypeScriptCompileItemName)
        {
        }

        public override bool CodeFileSupported(CodeFile codeFile)
        {
            return codeFile is CodeFileLtsText;
        }
    }
}
