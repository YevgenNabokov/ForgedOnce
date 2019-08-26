using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Plugins;
using Game08.Sdk.CodeMixer.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests.TestClasses
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

        protected override void Implementation(CodeFileCSharp input, TestPluginMetadata metadata, IMetadataWriter outputMetadataWriter)
        {
            var code = this.Outputs[outputStreamName].CreateCodeFile("Output.cs") as CodeFileCSharp;

            
        }
    }
}
