using NUnit.Framework;
using Seasar.Quill.Attr;
using Seasar.Quill.Parts.Container.ImplTypeFactory.Impl;

namespace Seasar.Tests.Quill.Preset.ImplType
{
    [TestFixture]
    public class ImplementationAttributeImplTypeFactoryTest : QuillTestBase
    {
        [Test]
        public void TestGetImplTypeWithImplType()
        {
            // Arrange
            var actor = new ImplementationAttributeImplTypeFactory();

            // Act
            var actual = actor.GetImplType(typeof(ImplAttrAttachedWithImplTypeClass));

            // Assert
            Assert.AreEqual(typeof(string), actual);
        }

        [Test]
        public void TestGetImplTypeWithoutImplType()
        {
            // Arrange
            var actor = new ImplementationAttributeImplTypeFactory();

            // Act
            var actual = actor.GetImplType(typeof(ImplAttrAttachedWithoutImplTypeClass));

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void TestGetImplTypeNotAttachedImplementationAttribute()
        {
            // Arrange
            var actor = new ImplementationAttributeImplTypeFactory();

            // Act
            var actual = actor.GetImplType(typeof(ImplAttrNotAttachedClass));

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void TestDispose()
        {
            var actor = new ImplementationAttributeImplTypeFactory();
            actor.Dispose(); // 処理なしなので、呼んでも特に例外とはならないことを確認
        }
        
        [Implementation(ImplType=typeof(string))]
        private class ImplAttrAttachedWithImplTypeClass
        { }

        [Implementation]
        private class ImplAttrAttachedWithoutImplTypeClass
        { }

        private class ImplAttrNotAttachedClass
        { }
    }
}
