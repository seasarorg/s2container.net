using NUnit.Framework;
using Seasar.Quill.Preset.Scope;

namespace Seasar.Tests.Quill.Preset.Scope
{
    [TestFixture]
    public class PrototypeFactoryTest : QuillTestBase
    {
        [Test]
        public void TestCreateInstance()
        {
            // Arrange
            var actor = new PrototypeFactory();
            var testType = typeof(TestEntity);

            // Act
            var component1 = actor.CreateInstance(testType);
            var component2 = actor.CreateInstance(testType);

            // Arrange
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.AreNotSame(component1, component2); // 毎回newしているので別インスタンス
        }

        private class TestEntity
        { }
    }
}
