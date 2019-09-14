using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl.MsBuild
{
    public class GlslMsBuildStoreAdapter : DefaultItemStoreAdapter
    {
        public const string ContentItemName = "Content";

        public GlslMsBuildStoreAdapter(IFileSystem fileSystem)
            : base(fileSystem, ContentItemName)
        {
        }

        public override bool CodeFileSupported(CodeFile codeFile)
        {
            return codeFile is CodeFileGlsl;
        }
    }
}
