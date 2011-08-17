using System;
using System.Collections.Generic;
using System.Drawing;
using ConfigReader.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_config_reader_reads_configuration_with_different_data_types : ConfigurationReaderSpecificationBase
    {
        [SetUp]
        protected override void Context()
        {
            base.Context();

            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"ConfigurationAdapter2.BoolProperty", true.ToString()},
                        {"ConfigurationAdapter2.StringProperty", "2prop1"},
                        {"ConfigurationAdapter1.IntegerProperty", 2.ToString()},
                        {"ConfigurationAdapter1.StringProperty", "1prop1"},
                        {"ConfigurationAdapter1.ColorProperty", "#FFFFFF"},
                        {"ConfigurationAdapter1.AddressProperty", "http://localhost"}
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            this.mocks.ReplayAll();
        }

        [Test]
        public void it_should_convert_the_string_values_to_the_data_types()
        {
            var configReader = new ConfigurationReader(this.configSource);

            configReader.
                SetupConfigOf<IConfigurationAdapter1>().
                SetupConfigOf<IConfigurationAdapter2>();

            IConfigurationBrowser configurations = configReader.ConfigBrowser;

            var configurationAdapter1 = configurations.Get<IConfigurationAdapter1>();

            Assert.IsNotNull(configurationAdapter1);
            Assert.IsNotNull(configurations.Get<IConfigurationAdapter2>());

            Assert.AreEqual("1prop1", configurationAdapter1.StringProperty);
            Assert.AreEqual(2, configurationAdapter1.IntegerProperty);
            Assert.AreEqual(Color.White, configurationAdapter1.ColorProperty);
            Assert.AreEqual(new Uri("http://localhost"), configurationAdapter1.AddressProperty);
        }
    }
}
