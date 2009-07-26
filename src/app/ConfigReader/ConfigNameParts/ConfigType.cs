using System.Collections.ObjectModel;

namespace ConfigReader.ConfigNameParts
{
    public class ConfigType
    {
        public ConfigType(string name)
        {
            this.Name = name;
            this.Properties = new ConfigPropertyCollection();
        }

        public string Name { get; private set; }
        public ConfigPropertyCollection Properties { get; private set; }

        public void Add(string name, string value)
        {
            this.Properties.Add(new ConfigProperty {Name = name, Value = value});
        }
    }

    public class ConfigTypesCollection : KeyedCollection<string, ConfigType>
    {
        public new ConfigType this[string name]
        {
            get
            {
                if (this.Contains(name))
                    return base[name];
                
                if (this.Contains(name.TrimStart('I')))
                    return base[name.TrimStart('I')];

                return null;
            }
        }

        protected override string GetKeyForItem(ConfigType item)
        {
            return item.Name;
        }
    }
}