using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_the_config_reader_get_default_vaules : ConfigurationReaderSpecificationBase
    {
        [SetUp]
        protected override void Context()
        {
            base.Context();

            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"ConfigurationAdapter1.StringProperty", "1prop1"},
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            this.mocks.ReplayAll();
        }

        [Test]
        public void it_will_use_the_default_values_when_no_value_was_specified_in_the_configuration()
        {
            var configReader = new ConfigurationReader(this.configSource).SetupConfigOf<IConfigurationAdapter1>(new ConfigurationAdapter1DefaultValues());

            var browser = configReader.ConfigBrowser.Get<IConfigurationAdapter1>();

            Assert.AreEqual("1prop1", browser.StringProperty);
            Assert.AreEqual(12, browser.IntegerProperty);
            Assert.AreEqual(Color.DimGray, browser.ColorProperty);
        }

        public class ConfigurationAdapter1DefaultValues : IConfigurationAdapter1
        {
            public string StringProperty
            {
                get { return "Default"; }
            }

            public int IntegerProperty
            {
                get { return 12; }
            }

            public Color ColorProperty
            {
                get { return Color.DimGray; }
            }

            public Uri AddressProperty
            {
                get { return null; }
            }
        }
    }
}