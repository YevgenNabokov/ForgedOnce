using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgedOnce.Core.Plugins
{
    public abstract class GroupedCodeGenerationPlugin<TSettings, TInputParameters, TCodeFile> : CodeGenerationPluginBase<TSettings, TInputParameters, TCodeFile>, ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public const string DefaultGroupName = "ALL_FILES";

        public IFileGroupAggregator<TCodeFile> InputAggregator;

        public override void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader, ILogger logger)
        {
            if (input.Any(file => !(file is TCodeFile)))
            {
                throw new InvalidOperationException($"Plugin supports only {typeof(TCodeFile)} as input.");
            }

            var inputConcrete = input.Cast<TCodeFile>();

            List<IFileGroup<TCodeFile, GroupItemDetails>> groups = null;

            if (this.InputAggregator != null)
            {
                groups = new List<IFileGroup<TCodeFile, GroupItemDetails>>(this.InputAggregator.Aggregate(inputConcrete));
            }
            else
            {
                var defaultGroup = new FileGroup<TCodeFile, GroupItemDetails>()
                {
                    Name = DefaultGroupName
                };

                foreach (var i in inputConcrete)
                {
                    defaultGroup.Files.Add(i, new GroupItemDetails());
                }

                groups = new List<IFileGroup<TCodeFile, GroupItemDetails>>() { defaultGroup };
            }

            foreach (var group in groups)
            {
                var inputGroup = new FileGroup<TCodeFile, GroupItemDetailsParametrized<TInputParameters>>();
                inputGroup.Name = group.Name;

                foreach (var file in group.Files)
                {
                    var metadata = this.Preprocessor.GenerateMetadata(file.Key, this.Settings, metadataReader, logger, group);
                    inputGroup.Files.Add(file.Key, new GroupItemDetailsParametrized<TInputParameters>(metadata, file.Value.GroupingTags));
                }

                this.Implementation(inputGroup, metadataRecorder, logger);
            }
        }

        protected abstract void Implementation(FileGroup<TCodeFile, GroupItemDetailsParametrized<TInputParameters>> group, IMetadataRecorder metadataRecorder, ILogger logger);
    }
}
