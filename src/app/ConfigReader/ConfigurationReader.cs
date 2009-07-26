using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Components.DictionaryAdapter;
using ConfigReader.ConfigNameParts;
using ConfigReader.ConfigurationSources;
using ConfigReader.Interfaces;

namespace ConfigReader
{
    public class ConfigurationReader
    {
        private readonly IConfigurationSource configurationSource;
        private readonly DictionaryAdapterFactory dictionaryAdapterFactory = new DictionaryAdapterFactory();
        private readonly DefaultConfigurationBrowser configBrowser = new DefaultConfigurationBrowser();
        private ConfigTypesCollection configTypes;
        private Dictionary<Type, Func<string, object>> customConvertions;

        /// <summary>
        /// Creates the ConfigurationReader based on the Application Configuration file.
        /// </summary>
        public ConfigurationReader()
            :this(new AppConfigSource())
        {
        }


        public ConfigurationReader(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) 
                throw new ArgumentNullException("configurationSource");

            this.configurationSource = configurationSource;
            Initialize();
        }

        private void Initialize()
        {
            configTypes = ConfigTransformer.
                ForConfigSource(configurationSource.GetConfigDictionary()).
                Transform();
        }

        public ConfigurationReader SetupConfigOf<T>()
        {
            var cnfg = typeof (T);

            var config = configTypes[cnfg.Name];

            var configurationForT = config.Properties.GetValuesDictionary();

            var configConverter = new ConfigConverter(typeof (T), CustomConvertions);

            var convertedConfiguration = configConverter.ConvertConfigProperties(configurationForT);
            
            configBrowser.AddConfigAdapter(
                typeof(T),
                dictionaryAdapterFactory.GetAdapter<T>(convertedConfiguration));

            return this;
        }

        public ConfigurationReader SetupConfigOf<T>(object defaultValues)
        {
            FillConfigTypesWithDefaultValues<T>(defaultValues);
            return SetupConfigOf<T>();
        }

        private void FillConfigTypesWithDefaultValues<T>(object defaultValues)
        {
            var configType = typeof (T);

            var typePublicProperties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            
            var config = configTypes[configType.Name];
            
            if(config == null)
                configTypes.Add(config = new ConfigType(configType.Name));

            var configurationForT = config.Properties;

            foreach (var propertyInfo in typePublicProperties)
            {
                var propertyName = propertyInfo.Name;

                if (!configurationForT.Contains(propertyName))
                {
                    ConfigProperty configProperty;
                    try
                    {
                        configProperty = new ConfigProperty
                                             {
                                                 Name = propertyName,
                                                 Value =
                                                     defaultValues.GetType().GetProperty(propertyName).GetValue(
                                                     defaultValues, null)
                                             };
                    }
                    catch (NullReferenceException)
                    {
                        throw new ConfigurationException(
                            String.Format("The default values object misses the property named '{0}' of type '{1}'.",
                                          propertyName, propertyInfo.PropertyType.FullName));
                    }

                    if (configProperty.Value != null &&
                        !propertyInfo.PropertyType.IsAssignableFrom(configProperty.Value.GetType()))
                        throw new ConfigurationException(
                            String.Format("The default values object misses the property named '{0}' of type '{1}'.",
                                          propertyName, propertyInfo.PropertyType.FullName));


                    configurationForT.Add(configProperty);
                }
            }
        }

        private void ValidateConfiguration()
        {
            
        }

        public IConfigurationBrowser ConfigBrowser
        {
            get
            {
                return configBrowser;
            }
        }

        public ConfigurationReader SetupCustomConverter<T>(Func<string, T> conversion)
        {
            CustomConvertions.Add(typeof (T), source => conversion(source) as object);
            return this;
        }

        private Dictionary<Type, Func<string, object>> CustomConvertions
        {
            get
            {
                if(customConvertions == null)
                    customConvertions = new Dictionary<Type, Func<string, object>>();

                return customConvertions;
            }
        }
    }
}