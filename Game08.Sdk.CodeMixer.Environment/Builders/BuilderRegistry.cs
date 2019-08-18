using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class BuilderRegistry : IBuilderProvider
    {
        private Dictionary<Type, Dictionary<string, object>> registry = new Dictionary<Type, Dictionary<string, object>>();

        public void Register<TResult>(IBuilder<TResult> builder)
        {
            if (!this.registry.ContainsKey(typeof(TResult)))
            {
                this.registry.Add(typeof(TResult), new Dictionary<string, object>());
            }

            var dict = this.registry[typeof(TResult)];

            if (dict.ContainsKey(builder.Name))
            {
                throw new InvalidOperationException($"Duplicate builder name registration {builder.Name} for {typeof(IBuilder<TResult>)}.");
            }

            dict.Add(builder.Name, builder);
        }

        public IBuilder<TResult> Resolve<TResult>(string name, bool throwIfNotFound = true)
        {
            if (this.registry.ContainsKey(typeof(TResult)))
            {
                if (this.registry[typeof(TResult)].ContainsKey(name))
                {
                    return this.registry[typeof(TResult)][name] as IBuilder<TResult>;
                }
            }

            if (throwIfNotFound)
            {
                throw new InvalidOperationException($"No builder was found for {typeof(TResult)} with name={name}.");
            }

            return null;
        }
    }
}
