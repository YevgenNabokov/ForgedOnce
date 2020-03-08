using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Plugins
{
    public abstract class CodeGenerationPluginBase<TSettings, TInputParameters, TCodeFile> : ICodeGenerationPlugin where TCodeFile : CodeFile
    {
        public IPluginPreprocessor<TCodeFile, TInputParameters, TSettings> Preprocessor;

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

        public abstract void Execute(IEnumerable<CodeFile> input, IMetadataRecorder metadataRecorder, IMetadataReader metadataReader, ILogger logger);

        protected abstract List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory);
    }
}
