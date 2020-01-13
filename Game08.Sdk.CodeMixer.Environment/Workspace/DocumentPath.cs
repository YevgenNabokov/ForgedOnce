using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class DocumentPath
    {
        public DocumentPath()
        {
        }

        public DocumentPath(string documentPathString)
        {
            var parts = documentPathString.Split('/');
            var folders = new string[parts.Length - 2];
            Array.Copy(parts, 1, folders, 0, parts.Length - 2);

            this.ProjectName = parts[0];
            this.ProjectFolders = folders;
            this.DocumentName = parts.Last();
        }

        public DocumentPath(string projectName, IEnumerable<string> projectFolders, string documentName)
        {
            this.ProjectName = projectName;
            this.ProjectFolders = projectFolders != null ? projectFolders.ToArray() : new string[0];
            this.DocumentName = documentName;
        }

        public string DocumentName;

        public string ProjectName;

        public string[] ProjectFolders;

        public override bool Equals(object obj)
        {
            var path = obj as DocumentPath;
            if (path == null)
            {
                return false;
            }

            return this.ProjectName == path.ProjectName
                && this.ProjectFolders.SequenceEqual(path.ProjectFolders)
                && this.DocumentName == path.DocumentName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (this.ProjectName != null ? this.ProjectName.GetHashCode() : 0);
                foreach (var folder in this.ProjectFolders)
                {
                    hash = hash * 23 + folder.GetHashCode();
                }

                hash = hash * 23 + (this.DocumentName != null ? this.DocumentName.GetHashCode() : 0);
                return hash;
            }
        }

        public override string ToString()
        {
            var joinedFolders = string.Join("/", this.ProjectFolders);
            if (!string.IsNullOrEmpty(joinedFolders))
            {
                return $"{this.ProjectName}/{joinedFolders}/{this.DocumentName}";
            }
            else
            {
                return $"{this.ProjectName}/{this.DocumentName}";
            }
        }
    }
}
