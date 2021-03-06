﻿using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class PipelineEnvironment : IPipelineEnvironment
    {
        protected List<ICodeStream> activeCodeStreams = new List<ICodeStream>();

        public List<ICodeFileEnvironmentHandler> Handlers = new List<ICodeFileEnvironmentHandler>();

        public PipelineEnvironment(IPipelineExecutionInfo pipelineExecutionInfo)
        {
            this.PipelineExecutionInfo = pipelineExecutionInfo;
        }

        public IPipelineExecutionInfo PipelineExecutionInfo
        {
            get;
            private set;
        }

        public IEnumerable<CodeFile> GetOutputs()
        {
            List<CodeFile> result = new List<CodeFile>();
            foreach (var handler in this.Handlers)
            {
                foreach (var file in handler.GetOutputs())
                {
                    if (!result.Contains(file))
                    {
                        result.Add(file);
                    }
                }
            }

            return result;
        }

        public void CodeStreamsDiscarded(IEnumerable<ICodeStream> streams)
        {
            foreach (var stream in streams)
            {
                if (this.activeCodeStreams.Contains(stream))
                {
                    foreach (var file in stream.Files)
                    {
                        this.HandlerFor(file.Language).Remove(file);
                    }

                    this.activeCodeStreams.Remove(stream);
                }
            }
        }

        public void CodeStreamsCompleted(IEnumerable<ICodeStream> streams)
        {
            foreach (var stream in streams)
            {
                if (!this.activeCodeStreams.Contains(stream))
                {
                    foreach (var file in stream.Files)
                    {
                        this.HandlerFor(file.Language).Add(file);
                    }

                    this.activeCodeStreams.Add(stream);
                }
            }
        }

        public void StoreForOutput(IEnumerable<CodeFile> files)
        {
            foreach (var file in files)
            {
                this.HandlerFor(file.Language).AddOutput(file);
            }
        }

        public void RefreshAndRecompile()
        {
            foreach (var handler in this.Handlers)
            {
                handler.RefreshAndRecompile();
            }
        }

        public ICodeStream CreateCodeStream(string language, string name, ICodeFileDestination codeFileLocationProvider = null)
        {
            return this.HandlerFor(language).CreateCodeStream(language, name, codeFileLocationProvider);
        }

        private ICodeFileEnvironmentHandler HandlerFor(string language)
        {
            foreach (var handler in this.Handlers)
            {
                if (handler.SupportsCodeLanguage(language))
                {
                    return handler;
                }
            }

            throw new InvalidOperationException($"Handler for {language} was not found in {nameof(PipelineEnvironment)}.");
        }

        public CodeFile ResolveCodeFile(string language, CodeFileLocation location)
        {
            return this.HandlerFor(language).ResolveExistingCodeFile(location);
        }

        public bool CanResolveCodeFile(string language, CodeFileLocation location)
        {
            return this.HandlerFor(language).CanResolveExistingCodeFile(location);
        }

        public ShadowFilter GetShadowFilter(string language)
        {
            return this.HandlerFor(language).ShadowFilter;
        }
    }
}
