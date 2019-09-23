using Game08.Sdk.CodeMixer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
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

        public DocumentPath DocumentPath;
    }
}
