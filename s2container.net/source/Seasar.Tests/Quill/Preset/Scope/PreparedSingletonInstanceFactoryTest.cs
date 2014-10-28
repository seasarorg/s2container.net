using NUnit.Framework;
using Seasar.Quill.Parts.Container.InstanceFactory.Impl;
using System;
using System.Collections.Generic;

namespace Seasar.Tests.Quill.Preset.Scope
{
    [TestFixture]
	public class PreparedSingletonInstanceFactoryTest : QuillTestBase
    {
        [Test]
        public void TestGetInstance()
        {
            // Arrange
            var preparedInstances = new Dictionary<Type, object>();
            preparedInstances.Add(typeof(TestEntity), new TestEntity());
            var factory = new PreparedSingletonInstanceFactory(preparedInstances);

            // Act
            var preparedComponent1 = factory.GetInstance(typeof(TestEntity));
            var preparedComponent2 = factory.GetInstance(typeof(TestEntity));
            var singletonComponent1 = factory.GetInstance(typeof(TestNotPrepared));
            var singletonComponent2 = factory.GetInstance(typeof(TestNotPrepared));
            
            // Assert
            Assert.IsNotNull(preparedComponent1);
            Assert.IsNotNull(singletonComponent1);   // PrepareされていなくてもSingletonFactory側で生成される
            Assert.AreSame(preparedComponent1, preparedComponent2);    // singletonなので同インスタンス
            Assert.AreSame(singletonComponent1, singletonComponent2);  // singletonなので同インスタンス
        }

        [Test]
        public void TestDispose()
        {
             // Arrange
            var preparedInstances = new Dictionary<Type, object>();
            preparedInstances.Add(typeof(TestEntity), new TestEntity());
            var factory = new PreparedSingletonInstanceFactory(preparedInstances);

            // Act
            var preparedComponent1 = factory.GetInstance(typeof(TestEntity));
            factory.Dispose();
            var preparedComponent2 = factory.GetInstance(typeof(TestEntity));
            
            // Assert
            Assert.IsNotNull(preparedComponent1);
            Assert.IsNotNull(preparedComponent2);   // PrepareされていなくてもSingletonFactory側で生成される
            Assert.AreNotSame(preparedComponent1, preparedComponent2);
        }

        private class TestEntity
        { }

        private class TestNotPrepared
        { }
    }
}
