﻿using Game08.Sdk.CodeMixer.Core;
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

        public override bool Equals(object obj)
        {
            var location = obj as WorkspaceCodeFileLocation;
            if (location == null)
            {
                return false;
            }

            return this.FilePath == location.FilePath
                && ((this.DocumentPath == null && location.DocumentPath == null)
                || (this.DocumentPath != null && this.DocumentPath.Equals(location.DocumentPath)));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (this.FilePath != null ? this.FilePath.GetHashCode() : 0);
                hash = hash * 23 + (this.DocumentPath != null ? this.DocumentPath.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
