using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_config_reader_reads_a_values_that_cant_be_casted_to_the_interface_datatype : ConfigurationReaderSpecificationBase
    {
        protected override void Context()
        {
            base.Context();

            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"ConfigurationAdapter1.IntegerProperty", 2.ToString()},
                        {"ConfigurationAdapter1.StringProperty", "1prop1"},
                        {"ConfigurationAdapter1.ColorProperty", "WrongValue"}
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            mocks.ReplayAll();
        }


        [Test]
        [ExpectedException(typeof(ConfigurationException), ExpectedMessage = "The property 'IConfigurationAdapter1.ColorProperty' could not be converted correctly.")]
        public void it_should_throw_an_exception_stating_the_property_name()
        {
            var configReader = new ConfigurationReader(configSource);

            configReader.SetupConfigOf<IConfigurationAdapter1>();
        }
    }
}