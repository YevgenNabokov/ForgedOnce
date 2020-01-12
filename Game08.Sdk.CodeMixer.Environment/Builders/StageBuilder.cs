using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
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
        private readonly ITypeLoader typeLoader;
        private readonly ILogger logger;

        public string Name => "DefaultStageBuilder";

        public StageBuilder(IBuilderProvider builderProvider, ITypeLoader typeLoader, ILogger logger)
        {
            this.builderProvider = builderProvider;
            this.typeLoader = typeLoader;
            this.logger = logger;
        }

        public StageContainer Build(JObject configuration)
        {
            var stageConfiguration = new StageConfiguration(configuration);
            StageContainer result = new StageContainer();

            result.InputSelector = this.builderProvider.Resolve<ICodeFileSelector>(stageConfiguration.Input.BuilderName).Build(stageConfiguration.Input.Configuration);
            if (stageConfiguration.Output != null)
            {
                result.FinalOutputSelector = this.builderProvider.Resolve<ICodeFileSelector>(stageConfiguration.Output.BuilderName).Build(stageConfiguration.Output.Configuration);
            }

            Dictionary<string, ICodeFileDestination> mappers = new Dictionary<string, ICodeFileDestination>();
            foreach (var mapper in stageConfiguration.CodeStreamMappers)
            {
                mappers.Add(mapper.Key, this.builderProvider.Resolve<ICodeFileDestination>(mapper.Value.BuilderName).Build(mapper.Value.Configuration));
            }

            result.OutputCodeStreamRenames = stageConfiguration.OutputCodeStreamRenames;

            result.CodeFileDestinations = mappers;

            result.Stage = this.BuildPluginStage(stageConfiguration.Plugin, stageConfiguration.Name);

            result.CleanDestinations = stageConfiguration.CleanDestinations;

            return result;
        }

        private Stage BuildPluginStage(PluginConfiguration pluginConfig, string name)
        {
            object preprocessor = null;

            if (pluginConfig.Preprocessor != null)
            {
                Type type = this.typeLoader.LoadType(pluginConfig.Preprocessor.Type);
                if (type is null)
                {
                    throw new InvalidOperationException($"Cannot resolve plugin preprocessor type {pluginConfig.Preprocessor.Type}.");
                }

                preprocessor = Activator.CreateInstance(type);
            }

            Type pluginFactoryType = this.typeLoader.LoadType(pluginConfig.PluginFactory.Type);
            if (pluginFactoryType is null)
            {
                throw new InvalidOperationException($"Cannot resolve plugin type {pluginConfig.PluginFactory.Type}.");
            }

            var factoryInterface = pluginFactoryType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICodeGenerationPluginFactory<,,>));

            if (factoryInterface == null)
            {
                throw new InvalidOperationException($"Plugin factory type should implement {typeof(ICodeGenerationPluginFactory<,,>)}.");
            }

            var pluginFactory = Activator.CreateInstance(pluginFactoryType);

            var method = factoryInterface.GetMethod(nameof(ICodeGenerationPluginFactory<object, object, CodeFile>.CreatePlugin));

            var parameters = new object[] { pluginConfig.PluginFactory.Configuration, preprocessor };

            var pluginInstance = method.Invoke(pluginFactory, parameters);

            return new Stage(this.logger)
            {
                Plugin = pluginInstance as ICodeGenerationPlugin,
                StageName = name
            };
        }
    }
}
