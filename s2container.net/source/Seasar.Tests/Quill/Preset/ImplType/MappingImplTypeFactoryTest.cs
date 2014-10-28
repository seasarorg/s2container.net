using NUnit.Framework;
using Seasar.Quill.Parts.Container.ImplTypeFactory;
using Seasar.Quill.Parts.Container.ImplTypeFactory.Impl;
using Seasar.Quill.Parts.Container.InstanceFactory;
using System;
using System.Collections.Generic;

namespace Seasar.Tests.Quill.Preset.ImplType
{
    [TestFixture]
    public class MappingImplTypeFactoryTest : QuillTestBase
    {
        [Test]
        public void TestGetImplType()
        {
            // Arrange
            var typeMap = new Dictionary<Type, Type>();
            typeMap.Add(typeof(IImplTypeFactory), typeof(MappingImplTypeFactory));
            var actor = new MappingImplTypeFactory(typeMap);

            // Act
            var actual = actor.GetImplType(typeof(IImplTypeFactory));

            // Assert
            Assert.AreEqual(typeof(MappingImplTypeFactory), actual);
        }

        [Test]
        public void TestGetImplTypeNotRegisterdType()
        {
            // Arrange
            var typeMap = new Dictionary<Type, Type>();
            typeMap.Add(typeof(IImplTypeFactory), typeof(MappingImplTypeFactory));
            var actor = new MappingImplTypeFactory(typeMap);

            // Act
            var actual = actor.GetImplType(typeof(IInstanceFactory));

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void TestDispose()
        {
            // Arrange
            var typeMap = new Dictionary<Type, Type>();
            typeMap.Add(typeof(IImplTypeFactory), typeof(MappingImplTypeFactory));
            var actor = new MappingImplTypeFactory(typeMap);

            // Act
            actor.Dispose();
            var actual = actor.GetImplType(typeof(MappingImplTypeFactory));

            // Assert
            Assert.IsNull(actual);
        }
    }
}
