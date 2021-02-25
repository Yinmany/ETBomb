using System;
using System.Collections.Generic;
using System.Reflection;

namespace AkaUI
{
    public class AkaBuilder
    {
        private List<Assembly> _assemblies = new List<Assembly>();
        private List<Action<Type>> _customFilters = new List<Action<Type>>();

        public static AkaBuilder Create()
        {
            return new AkaBuilder();
        }

        public void Build()
        {
            foreach (Assembly assembly in _assemblies)
            {
                var types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    foreach (Action<Type> filter in _customFilters)
                    {
                        filter(type);
                    }
                }
            }
        }

        public AkaBuilder AddAssemblyPart(params Assembly[] assemblies)
        {
            _assemblies.AddRange(assemblies);
            return this;
        }

        public AkaBuilder CustomAttributeFilter<T>(Action<Type, T> filter) where T : Attribute
        {
            _customFilters.Add(type =>
            {
                var attr = type.GetCustomAttribute<T>(true);
                if (attr != null)
                {
                    filter(type, attr);
                }
            });
            return this;
        }
    }
}