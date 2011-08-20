using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
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

		[Test]
		public void it_will_use_default_values_from_an_anonymous_object_when_no_value_was_specified_in_the_configuration()
		{
			var configReader = new ConfigurationReader(this.configSource).
				SetupConfigOf<IConfigurationAdapter1>(
				new
					{
						StringProperty = "Default",
						IntegerProperty = 12,
						ColorProperty = Color.DimGray,
						AddressProperty = (Uri) null
					});

			var browser = configReader.ConfigBrowser.Get<IConfigurationAdapter1>();

			Assert.AreEqual("1prop1", browser.StringProperty);
			Assert.AreEqual(12, browser.IntegerProperty);
			Assert.AreEqual(Color.DimGray, browser.ColorProperty);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConfigurationException), ExpectedMessage = "The default values object misses the property named 'AddressProperty' of type 'System.Uri'.")]
		public void it_will_throw_an_exception_when_an_anonymous_object_is_missing_a_property_defined_in_the_configuration_adapter_interface()
		{
			var configReader = new ConfigurationReader(this.configSource).
				SetupConfigOf<IConfigurationAdapter1>(
				new
					{
						StringProperty = "Default",
						IntegerProperty = 12,
						ColorProperty = Color.DimGray,
					});
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConfigurationException), ExpectedMessage = "The default values object misses the property named 'AddressProperty' of type 'System.Uri'.")]
		public void it_will_throw_an_exception_when_an_anonymous_object_has_a_property_with_the_wrong_type()
		{
			var configReader = new ConfigurationReader(this.configSource).
				SetupConfigOf<IConfigurationAdapter1>(
				new
					{
						StringProperty = "Default",
						IntegerProperty = 12,
						ColorProperty = Color.DimGray,
						AddressProperty = Assembly.GetExecutingAssembly()  // Arbitrary choise. Just for the test's sake...
					});
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