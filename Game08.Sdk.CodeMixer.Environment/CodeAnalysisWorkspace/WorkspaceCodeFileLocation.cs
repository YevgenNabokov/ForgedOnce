using Game08.Sdk.CodeMixer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceCodeFileLocation : CodeFileLocation
    {
        public WorkspaceCodeFileLocation()
        {            
        }

        public WorkspaceCodeFileLocation(CodeFileLocation location)            
        {
            this.FilePath = location.FilePath;
        }

        public string DocumentName;

        public string ProjectName;

        public string[] ProjectFolders;
    }
}
