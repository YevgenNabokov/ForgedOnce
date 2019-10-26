using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class NewFileCodeStreamProvider : ICodeStreamProvider
    {
        private readonly string language;
        private readonly string name;
        private readonly string[] files;
        private readonly IPipelineEnvironment pipelineEnvironment;

        public NewFileCodeStreamProvider(
            string language,
            string name,
            string[] files,
            IPipelineEnvironment pipelineEnvironment)
        {
            this.language = language;
            this.name = name;
            this.files = files;
            this.pipelineEnvironment = pipelineEnvironment;
        }
            

        public IEnumerable<ICodeStream> RetrieveCodeStreams()
        {
            var result = this.pipelineEnvironment.CreateCodeStream(this.language, this.name);
            foreach (var file in this.files)
            {
                result.CreateCodeFile(file);
            }

            return new ICodeStream[] { result };
        }
    }
}
