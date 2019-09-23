using Game08.Sdk.CodeMixer.Environment.Workspace;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IWorkspaceManager : IWorkspaceManagerBase
    {
        IEnumerable<TReference> GetMetadataReferences<TReference>(Guid? projectId = null) where TReference : MetadataReference;

        IEnumerable<List<string>> GetProjectsDependencyChains(IEnumerable<string> projectNames);

        Project FindProjectByAssemblyName(string assemblyName);

        Project FindProject(string projectName);

        Project FindProject(Guid id);

        Document FindDocument(DocumentPath documentPath);

        Document FindDocumentByFilePath(string filePath);

        Document FindDocumentByDocumentPath(DocumentPath documentPath);

        Document AddCodeFile(DocumentPath documentPath, string sourceCodeText, string filePath = null);

        void ReplaceDocumentText(Guid documentId, string newText);

        void RemoveCodeFile(DocumentPath documentPath);

        IWorkspaceManager CreateAdHocClone();
    }
}
