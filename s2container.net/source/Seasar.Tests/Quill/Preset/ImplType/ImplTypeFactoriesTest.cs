using NUnit.Framework;
using Seasar.Quill.Attr;
using Seasar.Quill.Parts.Container.ImplTypeFactory;
using Seasar.Quill.Parts.Container.ImplTypeFactory.Impl;
using Seasar.Quill.Parts.Container.InstanceFactory;
using System;
using System.Collections.Generic;

namespace Seasar.Tests.Quill.Preset.ImplType
{
    [TestFixture]
    public class ImplTypeFactoriesTest : QuillTestBase
    {
        [Test]
        public void TestGetImplType_HitOnFirstImplTypeFactory()
        {
            // Arrange
            var actor = GetFactory();

            // Act
            var actual = actor.GetImplType(typeof(IImplTypeFactory));

            // Assert
            Assert.AreEqual(typeof(MappingImplTypeFactory), actual);
        }

        [Test]
        public void TestGetImplType_HitOnSecondImplTypeFactory()
        {
            // Arrange
            var actor = GetFactory();

            // Act
            var actual = actor.GetImplType(typeof(ImplementClass));

            // Assert
            Assert.AreEqual(typeof(string), actual);
        }

        [Test]
        public void TestGetImplType_NotFound()
        {
            // Arrange
            var actor = GetFactory();

            // Act
            var actual = actor.GetImplType(typeof(IInstanceFactory));

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void TestDispose()
        {
            // Arrange
            var actor = GetFactory();

            // Act
            actor.Dispose();
            var actual = actor.GetImplType(typeof(IImplTypeFactory));

            // Assert
            Assert.IsNull(actual);
        }

        private ImplTypeFactories GetFactory()
        {
            var typeMap = new Dictionary<Type, Type>();
            typeMap.Add(typeof(IImplTypeFactory), typeof(MappingImplTypeFactory));
            var factory1 = new MappingImplTypeFactory(typeMap);
            var factory2 = new ImplementationAttributeImplTypeFactory();
            var actor = new ImplTypeFactories();
            actor.AddFactory(factory1);
            actor.AddFactory(factory2);
            return actor;
        }

        [Implementation(ImplType=typeof(string))]
        private class ImplementClass
        { }
    }
}
