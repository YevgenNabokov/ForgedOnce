using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Plugins
{
    public abstract class CodeGenerationPlugin<TSettings, TInputParameters, TCodeFile> : CodeGenerationPluginBase<TSettings, TInputParameters, TCodeFile>, ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public override void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader, ILogger logger)
        {
            foreach (var file in input)
            {
                if (!(file is TCodeFile))
                {
                    throw new InvalidOperationException($"Plugin supports only {typeof(TCodeFile)} as input.");
                }

                var codeFile = (TCodeFile)file;

                var metadata = this.Preprocessor.GenerateParameters(codeFile, this.Settings, metadataReader, logger);

                this.Implementation(codeFile, metadata, metadataRecorder, logger);
            }
        }

        protected abstract void Implementation(TCodeFile input, TInputParameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger);
    }
}
