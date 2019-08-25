using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IWorkspaceManager
    {
        IEnumerable<List<Guid>> GetProjectsDependencyChains(IEnumerable<Guid> projectIds);

        Project FindProject(string projectName);

        Project FindProject(Guid id);

        Document FindDocument(Guid documentId);

        Document FindDocumentByFilePath(string filePath);

        Document FindDocumentByDocumentPath(string documentPath);

        Document AddCodeFile(Guid projectId, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null);

        void RemoveCodeFile(Guid documentId);

        IEnumerable<string> DocumentPaths
        {
            get;
        }
    }
}
