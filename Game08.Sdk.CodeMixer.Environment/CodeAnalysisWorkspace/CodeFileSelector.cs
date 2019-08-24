using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class CodeFileSelector : ICodeFileSelector
    {
        private Dictionary<string, string[]> filters;

        public CodeFileSelector(Dictionary<string, string[]> filters)
        {
            this.filters = filters;
        }

        public IEnumerable<CodeFile> Select(IEnumerable<ICodeStream> streams)
        {
            List<CodeFile> result = new List<CodeFile>();

            foreach (var stream in streams)
            {
                foreach (var filter in this.filters)
                {
                    if (PathMaskHelper.PathMatchMask(stream.Name, filter.Key))
                    {
                        foreach (var file in stream.Files)
                        {
                            foreach (var mask in filter.Value)
                            {
                                if (PathMaskHelper.PathMatchMask(file.Name, mask))
                                {
                                    if (!result.Contains(file))
                                    {
                                        result.Add(file);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
