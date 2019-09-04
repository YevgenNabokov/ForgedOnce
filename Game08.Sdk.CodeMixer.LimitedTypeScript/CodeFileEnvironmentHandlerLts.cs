﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.LTS.Builder.TypeData;
using Game08.Sdk.LTS.Model;
using Game08.Sdk.LTS.ModelToCode;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileEnvironmentHandlerLts : CodeFileEnvironmentHandler
    {
        private readonly IFileSystem fileSystem;

        private LtsTypeRepository typeRepository;

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem)
            : this(fileSystem, new CodeFileStorageHandlerLts(), new CodeFileCompilationHandlerLts())
        {            
        }

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem, ICodeFileStorageHandler codeFileStorageHandler, ICodeFileCompilationHandler codeFileCompilationHandler)
            : base(codeFileStorageHandler, codeFileCompilationHandler)
        {
            this.fileSystem = fileSystem;
            this.typeRepository = new LtsTypeRepository();
        }

        public override IEnumerable<Core.CodeFile> GetOutputs()
        {
            var models = base.GetOutputs().Cast<CodeFileLtsModel>().ToList();

            var task = new CodeGenerationTask()
            {
                Files = new List<LTS.Model.CodeFile>()
            };

            string commonRootPath = null;
            foreach (var file in models)
            {
                var path = file.GetPath();
                if (this.fileSystem.Path.IsPathRooted(path))
                {
                    if (commonRootPath == null)
                    {
                        commonRootPath = path;
                    }
                    else
                    {
                        commonRootPath = PathMaskHelper.GetCommonRootPath(commonRootPath, path, this.fileSystem);
                    }
                }
            }

            var root = this.fileSystem.Path.GetTempPath();
            Dictionary<string, CodeFileLtsModel> fileNameMappings = new Dictionary<string, CodeFileLtsModel>();
            foreach (var file in models)
            {
                var tempPath = this.fileSystem.Path.IsPathRooted(file.GetPath()) ? file.GetPath().Substring(commonRootPath.Length) : file.GetPath();

                fileNameMappings.Add(tempPath, file);

                task.Files.Add(new LTS.Model.CodeFile()
                {
                    RootNode = file.Model,
                    IsDefinitionFile = file.IsDefinition,
                    FileName = tempPath
                });
            }

            task.Types = this.typeRepository.GetTypeCache();

            var launcher = new Launcher(root);
            var output = launcher.Execute(task);

            List<CodeFileLtsText> result = new List<CodeFileLtsText>();
            foreach (var map in fileNameMappings)
            {
                result.Add(new CodeFileLtsText(map.Value.Id, map.Value.Name)
                {
                    Location = map.Value.Location,
                    SourceCodeText = this.fileSystem.File.ReadAllText(this.fileSystem.Path.Combine(root, map.Key))
                });
            }

            return result;
        }

        protected override Core.CodeFile CreateCodeFile(string id, string name)
        {
            return new CodeFileLtsModel(id, name, this.typeRepository);
        }
    }
}