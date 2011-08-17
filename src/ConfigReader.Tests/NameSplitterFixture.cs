using NUnit.Framework;

namespace ConfigReader.Tests
{
    [TestFixture]
    public class NameSplitterFixture
    {
        private NameSplitter splitter;

        [SetUp]
        public void Setup()
        {
            splitter = new NameSplitter();
        }

        [Test]
        public void Split_ValidNameConsistsOfTwoPartsSeparatedByDot_ReturnsTheFirstPartAndScondPart()
        {
            var name = "TheTypeName.TheKey";

            SplittedName splitted = splitter.Split(name);

            Assert.IsNotNull(splitted);
            Assert.AreEqual("TheTypeName", splitted.TypeName);
            Assert.AreEqual("TheKey", splitted.Key);
        }

        [Test]
        public void Split_NameThatHasNoDot_ShouldReturnNull()
        {
            var name = "NonValidName";

            SplittedName splitted = splitter.Split(name);

            Assert.IsNull(splitted);
        }

        [Test]
        public void Split_NameIsNull_ShouldReturnNull()
        {
            SplittedName splitted = splitter.Split(null);

            Assert.IsNull(splitted);
        }
    }
}