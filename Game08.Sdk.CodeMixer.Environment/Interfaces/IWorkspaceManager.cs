using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IWorkspaceManager
    {
        IEnumerable<TReference> GetMetadataReferences<TReference>(Guid? projectId = null) where TReference : MetadataReference;

        IEnumerable<List<string>> GetProjectsDependencyChains(IEnumerable<string> projectNames);

        Project FindProjectByAssemblyName(string assemblyName);

        Project FindProject(string projectName);

        Project FindProject(Guid id);

        Document FindDocument(string projectName, string[] projectFolders, string documentName);

        Document FindDocumentByFilePath(string filePath);

        Document FindDocumentByDocumentPath(string documentPath);

        Document AddCodeFile(string projectName, IEnumerable<string> projectFolders, string name, string sourceCodeText, string filePath = null);

        void ReplaceDocumentText(Guid documentId, string newText);

        void RemoveCodeFile(string projectName, string[] projectFolders, string documentName);

        IEnumerable<string> DocumentPaths
        {
            get;
        }
    }
}
