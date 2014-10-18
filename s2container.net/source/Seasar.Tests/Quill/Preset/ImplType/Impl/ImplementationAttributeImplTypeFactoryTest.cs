using NUnit.Framework;
using Seasar.Quill.Attr;
using Seasar.Quill.Preset.Factory.Impl;

namespace Seasar.Tests.Quill.Preset.ImplType.Impl
{
    [TestFixture]
    public class ImplementationAttributeImplTypeFactoryTest
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
