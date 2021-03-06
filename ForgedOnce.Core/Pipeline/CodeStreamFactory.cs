﻿using ForgedOnce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class CodeStreamFactory : ICodeStreamFactory
    {
        private readonly IPipelineEnvironment pipelineEnvironment;
        private readonly Dictionary<string, ICodeFileDestination> codeFileLocationProviders;

        public CodeStreamFactory(IPipelineEnvironment pipelineEnvironment, Dictionary<string, ICodeFileDestination> codeFileLocationProviders)
        {
            this.pipelineEnvironment = pipelineEnvironment;
            this.codeFileLocationProviders = codeFileLocationProviders;
        }

        public ICodeStream CreateCodeStream(string language, string name)
        {
            if (!this.codeFileLocationProviders.ContainsKey(name))
            {
                throw new InvalidOperationException($"No code file location mapped for code stream {name}.");
            }

            return this.pipelineEnvironment.CreateCodeStream(language, name, this.codeFileLocationProviders[name]);
        }
    }
}
