using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConfigReader
{
    internal class ConfigConverter
    {
        private readonly Type type;
        private readonly Dictionary<Type, Func<string, object>> customConversions;
        private readonly Dictionary<string, PropertyInfo> propertiesOfType;

        public ConfigConverter(Type type, Dictionary<Type, Func<string, object>> customConversions)
        {
            if (type == null) 
                throw new ArgumentNullException("type");
            
            this.type = type;
            this.customConversions = customConversions;
            propertiesOfType = type.GetProperties().ToDictionary(prop=>prop.Name);
        }

        public IDictionary ConvertConfigProperties(IDictionary<string, object> configurationForT)
        {
            var result = new Dictionary<string, object>(configurationForT.Count);

            foreach(var pair in configurationForT)
            {
                var name = pair.Key;

                var propertyInfo = propertiesOfType[name];

                var value = ConvertValue(pair, name, propertyInfo);

                result.Add(name, value);
            }
            return result;
        }

        private object ConvertValue(KeyValuePair<string, object> pair, string name, PropertyInfo propertyInfo)
        {
            object value;

            if (customConversions != null && customConversions.ContainsKey(propertyInfo.PropertyType) && pair.Value is string)
            {
                try
                {
                    value = customConversions[propertyInfo.PropertyType](pair.Value as string);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(
                        String.Format("The property '{0}.{1}' could not be converted correctly.", type.Name, name),
                        ex);
                }
            }
            else if (pair.Value == null || propertyInfo.PropertyType.IsAssignableFrom(pair.Value.GetType()))
            {
                value = pair.Value;
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);

                try
                {
                    value = converter.ConvertFrom(pair.Value);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(
                        String.Format("The property '{0}.{1}' could not be converted correctly.", type.Name, name),
                        ex);
                }
            }
            return value;
        }
    }
}