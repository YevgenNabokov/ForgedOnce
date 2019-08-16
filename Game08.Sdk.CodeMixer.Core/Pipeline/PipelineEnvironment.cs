using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class PipelineEnvironment : IPipelineEnvironment
    {
        protected List<ICodeStream> activeCodeStreams = new List<ICodeStream>();

        public List<CodeFile> Output = new List<CodeFile>();

        public List<ICodeFileEnvironmentHandler> Handlers = new List<ICodeFileEnvironmentHandler>();

        public void CodeStreamsDiscarded(IEnumerable<ICodeStream> streams)
        {
            foreach (var stream in streams)
            {
                if (this.activeCodeStreams.Contains(stream))
                {
                    foreach (var file in stream.Files)
                    {
                        if (!this.Output.Contains(file))
                        {
                            foreach (var handler in this.HandlersFor(file.Language))
                            {
                                handler.Remove(file);
                            }
                        }
                    }

                    this.activeCodeStreams.Remove(stream);
                }
            }
        }

        public void CodeStreamsEmitted(IEnumerable<ICodeStream> streams)
        {
            foreach (var stream in streams)
            {
                if (!this.activeCodeStreams.Contains(stream))
                {
                    foreach (var file in stream.Files)
                    {
                        foreach (var handler in this.HandlersFor(file.Language))
                        {
                            handler.Add(file);
                        }
                    }

                    this.activeCodeStreams.Add(stream);
                }
            }
        }

        public void StoreForOutput(IEnumerable<CodeFile> files)
        {
            foreach (var file in files)
            {
                if (!this.Output.Contains(file))
                {
                    this.Output.Add(file);
                }
            }
        }

        public void RefreshAndRecompile()
        {
            foreach (var handler in this.Handlers)
            {
                handler.RefreshAndRecompile();
            }
        }

        private IEnumerable<ICodeFileEnvironmentHandler> HandlersFor(string language)
        {
            foreach (var handler in this.Handlers)
            {
                if (handler.SupportsCodeLanguage(language))
                {
                    yield return handler;
                }
            }
        }
    }
}
