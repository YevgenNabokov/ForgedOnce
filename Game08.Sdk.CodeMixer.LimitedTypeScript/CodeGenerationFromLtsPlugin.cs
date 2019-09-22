using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public abstract class CodeGenerationFromLtsPlugin<TSettings, TInputParameters> : CodeGenerationPlugin<TSettings, TInputParameters, CodeFileLtsModel>
    {
    }
}
