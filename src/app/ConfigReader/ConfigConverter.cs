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
        private readonly Dictionary<string, PropertyInfo> propertiesOfType;

        public ConfigConverter(Type type)
        {
            if (type == null) 
                throw new ArgumentNullException("type");
            
            this.type = type;
            propertiesOfType = type.GetProperties().ToDictionary(prop=>prop.Name);
        }

        public IDictionary ConvertConfigProperties(IDictionary<string, object> configurationForT)
        {
            var result = new Dictionary<string, object>(configurationForT.Count);

            foreach(var pair in configurationForT)
            {
                var name = pair.Key;

                var propertyInfo = propertiesOfType[name];
                object value;

                if (pair.Value == null || propertyInfo.PropertyType.IsAssignableFrom(pair.Value.GetType()))
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

                result.Add(name, value);
            }
            return result;
        }
    }
}