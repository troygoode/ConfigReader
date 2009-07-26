using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_one_adapter_is_from_config_and_one_from_default_config : ConfigurationReaderSpecificationBase
    {
        [Test]
        public void it_should_read_the_default_values_of_the_second_adapter()
        {
            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"ConfigurationAdapter1.IntegerProperty", 2.ToString()},
                        {"ConfigurationAdapter1.StringProperty", "1prop1"},
                        {"ConfigurationAdapter1.ColorProperty", "#FFFFFF"},
                        {"ConfigurationAdapter1.AddressProperty", "http://localhost"}
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            this.mocks.ReplayAll();

            var configReader = new ConfigurationReader(configSource);

            configReader.SetupConfigOf<IConfigurationAdapter1>(new DefaultAdapter1Values());
            configReader.SetupConfigOf<IConfigurationAdapter2>(new DefaultAdapter2Values());

            Assert.AreEqual(true, configReader.ConfigBrowser.Get<IConfigurationAdapter2>().BoolProperty);
        }
    }

    public class DefaultAdapter1Values : IConfigurationAdapter1
    {
        public string StringProperty
        {
            get { return "default"; }
        }

        public int IntegerProperty
        {
            get { return -1; }
        }

        public Color ColorProperty
        {
            get { return Color.DodgerBlue; }
        }

        public Uri AddressProperty
        {
            get { return new Uri("http://localhost"); }
        }
    }

    public class DefaultAdapter2Values : IConfigurationAdapter2
    {
        public string StringProperty
        {
            get { return "adapter 2 default"; }
        }

        public bool BoolProperty
        {
            get { return true; }
        }
    }
}