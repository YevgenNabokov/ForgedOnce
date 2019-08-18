using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public abstract class CodeGenerationPlugin<TSettings, TMetadata> : ICodeGenerationPlugin
    {
        public PluginSignature Signature;

        public PluginPreprocessor<TMetadata> Preprocessor;

        public TSettings Settings;

        private Dictionary<string, ICodeStream> Outputs = new Dictionary<string, ICodeStream>();

        public List<ICodeStream> InitializeOutputs()
        {            
            var outputs = this.CreateOutputs();
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
                this.Implementation(file, metadata, metadataWriter);
            }
        }

        protected abstract List<ICodeStream> CreateOutputs();

        protected abstract void Implementation(CodeFile input, TMetadata metadata, IMetadataWriter outputMetadataWriter);
    }
}
