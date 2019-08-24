using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IWorkspaceManager
    {
        Project FindProject(string projectName);

        Document FindDocumentByFilePath(string filePath);

        Document AddCodeFile(Guid projectId, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null);

        void RemoveCodeFile(Guid documentId);
    }
}
