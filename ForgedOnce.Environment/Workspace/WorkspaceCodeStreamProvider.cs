﻿using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace
{
    public class WorkspaceCodeStreamProvider : ICodeStreamProvider
    {
        private readonly string language;
        private readonly string name;
        private readonly ICodeFileResolver codeFileResolver;
        private readonly IWorkspaceManagerBase workspaceManager;
        private readonly IDocumentSelector documentSelector;
        private readonly IFileSystem fileSystem;
        private readonly string basePath;
        private readonly IFileSelector fileSelector;

        public WorkspaceCodeStreamProvider(
            string language,
            string name,
            ICodeFileResolver codeFileResolver,
            IWorkspaceManagerBase workspaceManager,
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
            IWorkspaceManagerBase workspaceManager,
            IDocumentSelector documentSelector)
        {
            this.language = language;
            this.name = name;
            this.codeFileResolver = codeFileResolver;
            this.workspaceManager = workspaceManager;
            this.documentSelector = documentSelector;
        }

        public IEnumerable<ICodeStream> RetrieveCodeStreams()
        {
            Dictionary<CodeFileLocation, CodeFile> codeFiles = new Dictionary<CodeFileLocation, CodeFile>();
            if (this.fileSelector != null)
            {
                foreach (var file in this.fileSelector.GetFiles(this.fileSystem, this.basePath))
                {
                    if (this.codeFileResolver.CanResolveCodeFile(this.language, file))
                    {
                        if (!codeFiles.ContainsKey(file))
                        {
                            codeFiles.Add(file, this.codeFileResolver.ResolveCodeFile(this.language, file));
                        }
                    }
                }
            }

            if (this.documentSelector != null)
            {
                foreach (var doc in this.documentSelector.GetDocuments(this.workspaceManager))
                {
                    if (!codeFiles.ContainsKey(doc))
                    {
                        codeFiles.Add(doc, this.codeFileResolver.ResolveCodeFile(this.language, doc));
                    }
                }
            }

            return new ICodeStream[] { new CodeStream(this.language, this.name, codeFiles.Values) };
        }
    }
}
