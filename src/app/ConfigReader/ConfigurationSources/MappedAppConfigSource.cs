using System.Collections.Generic;
using System.Configuration;
using ConfigReader.Interfaces;

namespace ConfigReader.ConfigurationSources
{
    public class MappedAppConfigSource : IConfigurationSource
    {
        private readonly Dictionary<string, string> configDictionary;

        public MappedAppConfigSource(string mappedConfig)
        {
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = mappedConfig };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            this.configDictionary = new Dictionary<string, string>(config.AppSettings.Settings.Count);

            foreach (var key in config.AppSettings.Settings.AllKeys)
            {
                this.configDictionary.Add(key, config.AppSettings.Settings[key].Value);
            }
        }

        public IDictionary<string, string> GetConfigDictionary()
        {
            return configDictionary;
        }
    }
}