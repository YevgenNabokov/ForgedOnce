using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public abstract class CodeGenerationPlugin<TSettings, TInputParameters, TCodeFile> : ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public IPluginPreprocessor<TInputParameters> Preprocessor;

        public TSettings Settings;

        protected Dictionary<string, ICodeStream> Outputs = new Dictionary<string, ICodeStream>();

        public PluginSignature Signature { get; protected set; }

        public List<ICodeStream> InitializeOutputs(ICodeStreamFactory codeStreamFactory)
        {            
            var outputs = this.CreateOutputs(codeStreamFactory);
            foreach (var output in outputs)
            {
                this.Outputs.Add(output.Name, output);
            }

            return outputs;
        }

        public void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader)
        {
            foreach (var file in input)
            {
                var metadata = this.Preprocessor.GenerateMetadata(file, metadataReader);

                if (!(file is TCodeFile))
                {
                    throw new InvalidOperationException($"Plugin supports only {typeof(TCodeFile)} as input.");
                }

                this.Implementation((TCodeFile)file, metadata, metadataRecorder);
            }
        }

        protected abstract List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory);

        protected abstract void Implementation(TCodeFile input, TInputParameters inputParameters, IMetadataRecorder metadataRecorder);
    }
}
