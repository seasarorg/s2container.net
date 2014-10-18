using NUnit.Framework;
using Seasar.Quill.Preset.Factory;
using Seasar.Quill.Preset.Factory.Impl;
using Seasar.Quill.Preset.Scope;
using System;
using System.Collections.Generic;

namespace Seasar.Tests.Quill.Preset.ImplType.Impl
{
    [TestFixture]
    public class MappingImplTypeFactoryTest
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
    }
}
