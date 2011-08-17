using System;
using System.Drawing;

namespace ConfigReader.Tests
{
    public interface IConfigurationAdapter1
    {
        string StringProperty { get; }
        int IntegerProperty { get; }
        Color ColorProperty { get; }
        Uri AddressProperty { get; }
    }
}