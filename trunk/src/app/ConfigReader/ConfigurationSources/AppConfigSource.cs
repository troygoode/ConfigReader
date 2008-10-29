using System.Collections.Generic;
using ConfigReader.Interfaces;
using System.Configuration;

namespace ConfigReader.ConfigurationSources
{
    public class AppConfigSource : IConfigurationSource
    {
        public IDictionary<string, string> GetConfigDictionary()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var result = new Dictionary<string, string>(appSettings.Count);

            foreach (var key in appSettings.AllKeys)
            {
                result.Add(key, appSettings[key]);    
            }
            return result;
        }
    }
}