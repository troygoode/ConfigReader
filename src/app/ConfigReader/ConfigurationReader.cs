using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            var configConverter = new ConfigConverter(typeof (T));

            var convertedConfiguration = configConverter.ConvertConfigProperties(configurationForT);
            
            configBrowser.AddConfigAdapter(
                typeof(T),
                dictionaryAdapterFactory.GetAdapter<T>(convertedConfiguration));

            return this;
        }

        public ConfigurationReader SetupConfigOf<T>(T defaultValues)
        {
            FillConfigTypesWithDefaultValues(defaultValues);
            return SetupConfigOf<T>();
        }

        private void FillConfigTypesWithDefaultValues<T>(T defaultValues)
        {
            var configType = typeof (T);

            var typePublicProperties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            
            var config = configTypes[configType.Name];
            var configurationForT = config.Properties;

            foreach (var propertyInfo in typePublicProperties)
            {
                var propertyName = propertyInfo.Name;

                if(!configurationForT.Contains(propertyName))
                {
                    configurationForT.Add(
                        new ConfigProperty
                            {
                                Name = propertyName,
                                Value = configType.GetProperty(propertyName).GetValue(defaultValues, null)
                            });
                }
            }
        }

        public IConfigurationBrowser ConfigBrowser
        {
            get
            {
                return configBrowser;
            }
        }
    }
}