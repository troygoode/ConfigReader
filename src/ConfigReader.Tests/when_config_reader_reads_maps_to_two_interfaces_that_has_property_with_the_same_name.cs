using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_config_reader_reads_maps_to_two_interfaces_that_has_property_with_the_same_name : ConfigurationReaderSpecificationBase
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
        public void it_should_map_the_correct_value_to_per_interface()
        {
            var configReader = new ConfigurationReader(configSource);

            var browser = configReader.SetupConfigOf<IConfigurationAdapter1>().SetupConfigOf<IConfigurationAdapter2>().ConfigBrowser;

            Assert.AreEqual("1prop1", browser.Get<IConfigurationAdapter1>().StringProperty);
            Assert.AreEqual("2prop1", browser.Get<IConfigurationAdapter2>().StringProperty);
        }
    }
}