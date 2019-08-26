using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public abstract class CodeGenerationPlugin<TSettings, TMetadata, TCodeFile> : ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public IPluginPreprocessor<TMetadata> Preprocessor;

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

        public void Execute(IEnumerable<CodeFile> input, IMetadataWriter metadataWriter, IMetadataReader metadataReader)
        {
            foreach (var file in input)
            {
                var metadata = this.Preprocessor.GenerateMetadata(file, metadataReader);

                if (!(file is TCodeFile))
                {
                    throw new InvalidOperationException($"Plugin supports only {typeof(TCodeFile)} as input.");
                }

                this.Implementation((TCodeFile)file, metadata, metadataWriter);
            }
        }

        protected abstract List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory);

        protected abstract void Implementation(TCodeFile input, TMetadata metadata, IMetadataWriter outputMetadataWriter);
    }
}
