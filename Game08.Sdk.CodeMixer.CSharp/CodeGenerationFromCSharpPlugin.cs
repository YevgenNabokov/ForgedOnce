using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public abstract class CodeGenerationFromCSharpPlugin<TSettings, TInputParameters> : CodeGenerationPlugin<TSettings, TInputParameters, CodeFileCSharp>
    {
    }
}
