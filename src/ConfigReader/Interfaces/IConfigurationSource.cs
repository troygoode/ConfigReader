using System.Collections.Generic;

namespace ConfigReader.Interfaces
{
    public interface IConfigurationSource
    {
        IDictionary<string, string> GetConfigDictionary();
    }
}