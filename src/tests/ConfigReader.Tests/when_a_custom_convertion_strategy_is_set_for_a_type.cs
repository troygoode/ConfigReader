using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class when_a_custom_convertion_strategy_is_set_for_a_type  : ConfigurationReaderSpecificationBase
    {
        protected override void Context()
        {
            base.Context();

            var configDictionaryToReturn =
                new Dictionary<string, string>
                    {
                        {"CustomConvertion.Address", "111.111.111.2"},
                        {"CustomConvertion.StringArray", "value1;value2;value3"}
                    };

            SetupResult.For(this.configSource.GetConfigDictionary()).Return(configDictionaryToReturn);

            mocks.ReplayAll();
        }


        [Test]
        public void it_should_use_it_to_convert_type_that_have_no_type_converter()
        {
            var configReader = new ConfigurationReader(this.configSource);

            configReader.
                SetupCustomConverter(source => source.Split(';')).
                SetupCustomConverter((string source) => IPAddress.Parse(source)).
                SetupConfigOf<ICustomConvertion>();


            var array = configReader.ConfigBrowser.Get<ICustomConvertion>().StringArray;
            Assert.AreEqual(3, array.Count());
            Assert.AreEqual("value3", array.ElementAt(2));

            var ipAddress = configReader.ConfigBrowser.Get<ICustomConvertion>().Address;
            Assert.IsNotNull(ipAddress);
            Assert.AreEqual("111.111.111.2", ipAddress.ToString());
        }

        public interface ICustomConvertion
        {
            IPAddress Address { get; }
            string[] StringArray { get; }
        }

    }
}