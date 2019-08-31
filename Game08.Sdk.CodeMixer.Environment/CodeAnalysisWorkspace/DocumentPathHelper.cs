using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public static class DocumentPathHelper
    {
        public static string GetPath(string projectName, IEnumerable<string> folders, string documentName)
        {
            var joinedFolders = string.Join("/", folders);
            return $"{projectName}/{joinedFolders}/{documentName}";
        }
    }
}
