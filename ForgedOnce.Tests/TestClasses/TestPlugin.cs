using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Tests.TestClasses
{
    public class TestPlugin : CodeGenerationPlugin<TestPluginSettings, TestPluginMetadata, CodeFileCSharp>
    {
        private const string outputStreamName = "OutputStream01";

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();

            result.Add(codeStreamFactory.CreateCodeStream(Languages.CSharp, outputStreamName));

            return result;
        }

        protected override void Implementation(CodeFileCSharp input, TestPluginMetadata metadata, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            var code = this.Outputs[outputStreamName].CreateCodeFile("Output.cs") as CodeFileCSharp;

            
        }
    }
}
