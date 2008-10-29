using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_config_reader_does_not_have_a_complete_config_in_the_source : ConfigurationReaderSpecificationBase
    {
        protected override void Context()
        {
            base.Context();

            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"ConfigurationAdapter1.StringProperty", "1prop1"}
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            this.mocks.ReplayAll();
        }

        [Test]
        public void it_should_return_the_default_value_for_the_type()
        {
            var configReader = new ConfigurationReader(this.configSource).SetupConfigOf<IConfigurationAdapter1>();
            
            Assert.AreEqual(0, configReader.ConfigBrowser.Get<IConfigurationAdapter1>().IntegerProperty);
            Assert.AreEqual(Color.Empty, configReader.ConfigBrowser.Get<IConfigurationAdapter1>().ColorProperty);
        }
    }
}