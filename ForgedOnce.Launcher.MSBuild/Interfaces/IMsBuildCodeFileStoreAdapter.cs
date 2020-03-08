using ForgedOnce.Core;
using ForgedOnce.Launcher.MSBuild.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild.Interfaces
{
    public interface IMsBuildCodeFileStoreAdapter
    {
        bool CodeFileSupported(CodeFile codeFile);

        bool ItemSupported(MsBuildItem item);

        void AddOrUpdate(CodeFile codeFile, MsBuildProject msBuildProject);

        void Remove(CodeFile codeFile, MsBuildProject msBuildProject);
    }
}
