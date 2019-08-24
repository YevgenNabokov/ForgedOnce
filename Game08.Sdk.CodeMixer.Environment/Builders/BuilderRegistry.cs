using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class BuilderRegistry : IBuilderProvider
    {
        private Dictionary<Type, Dictionary<string, object>> registry = new Dictionary<Type, Dictionary<string, object>>();

        private Dictionary<Type, object> noNameRegistry = new Dictionary<Type, object>();

        public void Register<TResult>(IBuilder<TResult> builder)
        {
            this.Register<TResult>(builder, builder.Name);
        }

        public void Register<TResult>(IBuilder<TResult> builder, string name)
        {
            if (name != null)
            {
                if (!this.registry.ContainsKey(typeof(TResult)))
                {
                    this.registry.Add(typeof(TResult), new Dictionary<string, object>());
                }

                var dict = this.registry[typeof(TResult)];

                if (dict.ContainsKey(name))
                {
                    throw new InvalidOperationException($"Duplicate builder name registration {name} for {typeof(IBuilder<TResult>)}.");
                }

                dict.Add(name, builder);
            }
            else
            {
                if (!this.noNameRegistry.ContainsKey(typeof(TResult)))
                {
                    this.noNameRegistry.Add(typeof(TResult), builder);
                }
                else
                {
                    throw new InvalidOperationException($"Duplicate builder nameless registration for {typeof(IBuilder<TResult>)}.");
                }
            }
        }

        public IBuilder<TResult> Resolve<TResult>(string name, bool throwIfNotFound = true)
        {
            if (name != null)
            {
                if (this.registry.ContainsKey(typeof(TResult)))
                {
                    if (this.registry[typeof(TResult)].ContainsKey(name))
                    {
                        return this.registry[typeof(TResult)][name] as IBuilder<TResult>;
                    }
                }
            }
            else
            {
                if (this.noNameRegistry.ContainsKey(typeof(TResult)))
                {
                    return this.noNameRegistry[typeof(TResult)] as IBuilder<TResult>;
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
