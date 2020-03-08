using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.TsLanguageServices.ModelBuilder.TypeData;
using ForgedOnce.TsLanguageServices.Model;
using ForgedOnce.TsLanguageServices.Model.DefinitionTree;
using ForgedOnce.TsLanguageServices.ModelToCode;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public class CodeFileEnvironmentHandlerLts : CodeFileEnvironmentHandler
    {
        private readonly string outputFolderName = "TsOutputs";

        private readonly IFileSystem fileSystem;

        private LtsTypeRepository typeRepository;

        public CodeFileEnvironmentHandlerLts(IFileSystem fileSystem, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
            : this(fileSystem, new CodeFileStorageHandlerLts(), new CodeFileCompilationHandlerLts(pipelineExecutionInfo, logger))
        {            
        }

        public CodeFileEnvironmentHandlerLts(
            IFileSystem fileSystem,
            ICodeFileStorageHandler codeFileStorageHandler,
            ICodeFileCompilationHandler codeFileCompilationHandler)
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
                Files = new List<TsLanguageServices.Model.CodeFile>()
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

            if (commonRootPath != null && this.fileSystem.Path.IsPathRooted(commonRootPath))
            {
                commonRootPath = this.fileSystem.Path.GetPathRoot(commonRootPath);
            }

            var root = this.fileSystem.Path.GetTempPath();
            var outputRoot = this.fileSystem.Path.Combine(root, $"{outputFolderName}_{Guid.NewGuid()}");
            Dictionary<string, CodeFileLtsModel> fileNameMappings = new Dictionary<string, CodeFileLtsModel>();
            foreach (var file in models)
            {
                var tempPath = this.fileSystem.Path.IsPathRooted(file.GetPath()) ? file.GetPath().Substring(commonRootPath.Length) : file.GetPath();

                fileNameMappings.Add(tempPath, file);

                task.Files.Add(new TsLanguageServices.Model.CodeFile()
                {
                    RootNode = file.Model.ToLtsModelFileRoot(),
                    IsDefinitionFile = file.IsDefinition,
                    FileName = tempPath
                });
            }

            task.Types = this.typeRepository.GetTypeCache();

            var launcher = new Launcher(outputRoot, root);
            var output = launcher.Execute(task);

            if (output.Errors.Count > 0)
            {
                var errorsText = string.Join("/r/n", output.Errors.Select(e => e.Message));
                throw new InvalidOperationException($"Errors occurred during TypeScript generation from intermediate model: \r\n{errorsText}");
            }

            List<CodeFileLtsText> result = new List<CodeFileLtsText>();
            foreach (var map in fileNameMappings)
            {
                result.Add(new CodeFileLtsText(map.Value.Id, map.Value.Name)
                {
                    Location = map.Value.Location,
                    SourceCodeText = this.fileSystem.File.ReadAllText(this.fileSystem.Path.Combine(outputRoot, map.Key))
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
