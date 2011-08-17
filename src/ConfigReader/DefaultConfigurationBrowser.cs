using System;
using System.Collections.Generic;
using ConfigReader.Interfaces;

namespace ConfigReader
{
    internal class DefaultConfigurationBrowser : IConfigurationBrowser
    {
        private readonly Dictionary<Type, object> configurations = new Dictionary<Type,object>();

        internal void AddConfigAdapter(Type type, object instance)
        {
            configurations.Add(type, instance);
        }

        #region IConfigurationBrowser Members

        public T Get<T>()
        {
            return (T)configurations[typeof (T)];
        }

        #endregion
    }
}
