using ConfigReader.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace ConfigReader.Tests
{
    public abstract class ConfigurationReaderSpecificationBase
    {
        protected MockRepository mocks;
        protected IConfigurationSource configSource;

        [SetUp]
        protected virtual void Context()
        {
            mocks = new MockRepository();
            configSource = this.mocks.DynamicMock<IConfigurationSource>();
        }
    }
}