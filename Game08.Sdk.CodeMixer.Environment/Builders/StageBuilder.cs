﻿using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Pipeline;
using Game08.Sdk.CodeMixer.Environment.Configuration;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class StageBuilder : IBuilder<StageContainer>
    {
        private readonly IBuilderProvider builderProvider;

        public string Name => "DefaultStageBuilder";

        public StageBuilder(IBuilderProvider builderProvider)
        {
            this.builderProvider = builderProvider;
        }

        public StageContainer Build(JObject configuration)
        {
            var stageConfiguration = new StageConfiguration(configuration);
            StageContainer result = new StageContainer();

            result.InputSelector = this.builderProvider.Resolve<ICodeFileSelector>(stageConfiguration.Input.BuilderName).Build(stageConfiguration.Input.Configuration);
            result.FinalOutputSelector = this.builderProvider.Resolve<ICodeFileSelector>(stageConfiguration.Output.BuilderName).Build(stageConfiguration.Output.Configuration);

            Dictionary<string, ICodeFileLocationProvider> mappers = new Dictionary<string, ICodeFileLocationProvider>();
            foreach (var mapper in stageConfiguration.CodeStreamMappers)
            {
                mappers.Add(mapper.Key, this.builderProvider.Resolve<ICodeFileLocationProvider>(mapper.Value.BuilderName).Build(mapper.Value.Configuration));
            }

            result.CodeFileLocationProviders = mappers;

            result.Stage = this.BuildPluginStage(stageConfiguration.Plugin, stageConfiguration.Name);

            return result;
        }

        private Stage BuildPluginStage(PluginConfiguration pluginConfig, string name)
        {
            object preprocessor = null;

            if (pluginConfig.Preprocessor != null)
            {
                Type type = Type.GetType(pluginConfig.Preprocessor.Type);
                if (type is null)
                {
                    throw new InvalidOperationException($"Cannot resolve plugin preprocessor type {pluginConfig.Preprocessor.Type}.");
                }

                preprocessor = Activator.CreateInstance(type);
            }

            Type pluginFactoryType = Type.GetType(pluginConfig.PluginFactory.Type);
            if (pluginFactoryType is null)
            {
                throw new InvalidOperationException($"Cannot resolve plugin type {pluginConfig.PluginFactory.Type}.");
            }

            if (!pluginFactoryType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICodeGenerationPluginFactory<,>)))
            {
                throw new InvalidOperationException($"Plugin factory type should implement {typeof(ICodeGenerationPluginFactory<,>)}.");
            }

            var pluginFactory = Activator.CreateInstance(pluginFactoryType);

            var method = typeof(ICodeGenerationPluginFactory<,>).GetMethod(nameof(ICodeGenerationPluginFactory<object, object>.CreatePlugin));

            var parameters = new object[] { pluginConfig.PluginFactory.Configuration, preprocessor };

            var pluginInstance = method.Invoke(pluginFactory, parameters);

            return new Stage()
            {
                Plugin = pluginInstance as ICodeGenerationPlugin,
                StageName = name
            };
        }
    }
}
