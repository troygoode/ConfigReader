using System.Collections.Generic;
using ConfigReader;
using NUnit.Framework;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class ConfigTransformerFixture
    {
        [Test]
        public void Parse_GetsAFlatDictionaryOfConfiguration_ShouldReturnAnHierarchicalStructure()
        {
            var inputConfiguration =
                new Dictionary<string, string>
                    {
                        {"StubInterface.StrProp", "the value"},
                        {"StubInterface.IntProp", "1"}
                    };


            var transformer = ConfigTransformer.ForConfigSource(inputConfiguration);

            var transformedDictionary = transformer.Transform();

            Assert.AreEqual(1, transformedDictionary.Count);
            Assert.IsTrue(transformedDictionary.Contains("StubInterface"));
            Assert.AreEqual(2, transformedDictionary["StubInterface"].Properties.Count);
        }
    }

    public interface IStubInterface
    {
        string StrProp { get; }
        string IntProp { get; }
    }
}
