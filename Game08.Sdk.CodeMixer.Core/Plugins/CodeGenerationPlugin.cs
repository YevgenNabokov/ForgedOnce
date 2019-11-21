using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public abstract class CodeGenerationPlugin<TSettings, TInputParameters, TCodeFile> : CodeGenerationPluginBase<TSettings, TInputParameters, TCodeFile>, ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public override void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader)
        {
            foreach (var file in input)
            {
                if (!(file is TCodeFile))
                {
                    throw new InvalidOperationException($"Plugin supports only {typeof(TCodeFile)} as input.");
                }

                var codeFile = (TCodeFile)file;

                var metadata = this.Preprocessor.GenerateMetadata(codeFile, metadataReader);

                this.Implementation(codeFile, metadata, metadataRecorder);
            }
        }

        protected abstract void Implementation(TCodeFile input, TInputParameters inputParameters, IMetadataRecorder metadataRecorder);
    }
}
