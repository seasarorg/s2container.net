using NUnit.Framework;
using Seasar.Quill.Parts.Container.InstanceFactory.Impl;

namespace Seasar.Tests.Quill.Preset.Scope
{
    [TestFixture]
    public class PrototypeInstanceFactoryTest : QuillTestBase
    {
        [Test]
        public void TestCreateInstance()
        {
            // Arrange
            var actor = new PrototypeInstanceFactory();
            var testType = typeof(TestEntity);

            // Act
            var component1 = actor.GetInstance(testType);
            var component2 = actor.GetInstance(testType);

            // Arrange
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.AreNotSame(component1, component2); // 毎回newしているので別インスタンス
        }

        [Test]
        public void TestDispose()
        {
            var actor = new PrototypeInstanceFactory();
            actor.Dispose(); // 処理なしなので、呼んでも例外は起こらないことを確認
        }

        private class TestEntity
        { }
    }
}
