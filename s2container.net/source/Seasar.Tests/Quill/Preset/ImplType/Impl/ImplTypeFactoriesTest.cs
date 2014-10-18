using NUnit.Framework;
using Seasar.Quill.Attr;
using Seasar.Quill.Preset.Factory;
using Seasar.Quill.Preset.Factory.Impl;
using Seasar.Quill.Preset.Scope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Tests.Quill.Preset.ImplType.Impl
{
    [TestFixture]
    public class ImplTypeFactoriesTest
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
