using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConfigReader.ConfigNameParts
{
    public class ConfigProperty
    {
        public string Name { get; internal set; }
        public object Value { get; internal set; }
    }

    public class ConfigPropertyCollection : KeyedCollection<string, ConfigProperty>
    {
        protected override string GetKeyForItem(ConfigProperty item)
        {
            return item.Name;
        }

        public Dictionary<string, object> GetValuesDictionary()
        {
            var result = new Dictionary<string, object>(this.Count);

            foreach (var item in this.Items)
            {
                result.Add(item.Name, item.Value);
            }

            return result;
        }
    }
}