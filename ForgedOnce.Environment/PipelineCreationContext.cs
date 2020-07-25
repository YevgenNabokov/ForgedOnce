using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment
{
    public class PipelineCreationContext : IPipelineCreationContext
    {
        public PipelineCreationContext(string basePath)
        {
            this.BasePath = basePath;
        }

        public string BasePath { get; private set; }
    }
}
