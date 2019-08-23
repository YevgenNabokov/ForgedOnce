﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class WorkspaceCodeStreamProvider
    {
        private readonly string language;
        private readonly string name;
        private readonly ICodeFileResolver codeFileResolver;
        private readonly IWorkspaceManager workspaceManager;
        private readonly IDocumentSelector documentSelector;
        private readonly IFileSystem fileSystem;
        private readonly string basePath;
        private readonly IFileSelector fileSelector;

        public WorkspaceCodeStreamProvider(
            string language,
            string name,
            ICodeFileResolver codeFileResolver,
            IWorkspaceManager workspaceManager,
            IFileSystem fileSystem,
            string basePath,
            IFileSelector fileSelector)
        {
            this.language = language;
            this.name = name;
            this.codeFileResolver = codeFileResolver;
            this.workspaceManager = workspaceManager;
            this.fileSystem = fileSystem;
            this.basePath = basePath;
            this.fileSelector = fileSelector;
        }

        public WorkspaceCodeStreamProvider(
            string language,
            string name,
            ICodeFileResolver codeFileResolver,
            IWorkspaceManager workspaceManager,
            IDocumentSelector documentSelector)
        {
            this.language = language;
            this.name = name;
            this.codeFileResolver = codeFileResolver;
            this.workspaceManager = workspaceManager;
            this.documentSelector = documentSelector;
        }

        public ICodeStream RetrieveCodeStream()
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            if (this.fileSelector != null)
            {
                foreach (var filePath in this.fileSelector.GetFiles(this.fileSystem, this.basePath))
                {
                    var document = this.workspaceManager.FindDocumentByFilePath(filePath);
                    if (document != null)
                    {
                        codeFiles.Add(this.codeFileResolver.ResolveCodeFile(this.language, new CodeFileLocation() { FilePath = filePath }));
                    }
                }
            }

            if (this.documentSelector != null)
            {
                foreach (var doc in this.documentSelector.GetDocuments(this.workspaceManager))
                {
                    codeFiles.Add(this.codeFileResolver.ResolveCodeFile(this.language, new WorkspaceCodeFileLocation()
                    {
                        DocumentId = doc.Id.Id                        
                    }));
                }
            }

            return new CodeStream(this.language, this.name, codeFiles);
        }
    }
}
