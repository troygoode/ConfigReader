using System;
using System.Collections.Generic;
using ConfigReader.ConfigNameParts;

namespace ConfigReader
{
    internal class ConfigTransformer
    {
		#region Fields (3) 

        private ConfigTypesCollection configurationEntriesCollection = null;
        private readonly IDictionary<string, string> inputConfiguration;
        private static readonly NameSplitter nameSplitter = new NameSplitter();

		#endregion Fields 

		#region Constructors (1) 

        private ConfigTransformer(IDictionary<string, string> inputConfiguration)
        {
            if (inputConfiguration == null) 
                throw new ArgumentNullException("inputConfiguration");

            this.inputConfiguration = inputConfiguration;
        }

		#endregion Constructors 

		#region Methods (8) 

		// Private Methods (6) 

        private void AddTheConfigurationValue(SplittedName splittedName, KeyValuePair<string, string> entry)
        {
            configurationEntriesCollection[splittedName.TypeName].Add(splittedName.Key, entry.Value);
        }

        private void CreateTheTransformedDictionary()
        {
            configurationEntriesCollection = new ConfigTypesCollection();
        }

        private void EnsureTransformedDictionaryHasKey(SplittedName splittedName)
        {
            if (!configurationEntriesCollection.Contains(splittedName.TypeName))
                configurationEntriesCollection.Add(new ConfigType(splittedName.TypeName));
        }

        private void SetupTheTransformedDictionary()
        {
            CreateTheTransformedDictionary();
            foreach(var entry in inputConfiguration)
            {
                var splittedName = SplitTheFlatKey(entry);
                
                if (!TheNameIsValid(splittedName)) 
                    continue;

                EnsureTransformedDictionaryHasKey(splittedName);
                AddTheConfigurationValue(splittedName, entry);
            }
        }

        private static SplittedName SplitTheFlatKey(KeyValuePair<string, string> entry)
        {
            return nameSplitter.Split(entry.Key);
        }

        private static bool TheNameIsValid(SplittedName splittedName)
        {
            return splittedName != null;
        }
		// Internal Methods (2) 

        internal static ConfigTransformer ForConfigSource(IDictionary<string, string> inputConfiguration)
        {
            return new ConfigTransformer(inputConfiguration);
        }

        internal ConfigTypesCollection Transform()
        {
            if (configurationEntriesCollection == null)
            {
                SetupTheTransformedDictionary();
            }
            return configurationEntriesCollection;
        }

		#endregion Methods 
    }
}