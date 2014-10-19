using NUnit.Framework;
using Seasar.Quill.Preset.Scope;
using System;
using System.Collections.Generic;

namespace Seasar.Tests.Quill.Preset.Scope
{
    [TestFixture]
	public class PreparedSingletonFactoryTest : QuillTestBase
    {
        [Test]
        public void TestInitialize()
        {
            // Arrange
            var prepared = new Dictionary<Type, object>();
            prepared.Add(typeof(TestEntity), new TestEntity());
            
            // Act
            PreparedSingletonFactory.Initialize(prepared);

            // Assert
            Assert.IsTrue(PreparedSingletonFactory.IsPrepared(typeof(TestEntity)));
            Assert.IsFalse(PreparedSingletonFactory.IsPrepared(typeof(TestNotPrepared)));
        }

        [Test]
        public void TestPrepare()
        {
            // Act
            PreparedSingletonFactory.Prepare(typeof(TestEntity), new TestEntity());

            // Assert
            Assert.IsTrue(PreparedSingletonFactory.IsPrepared(typeof(TestEntity)));
            Assert.IsFalse(PreparedSingletonFactory.IsPrepared(typeof(TestNotPrepared)));
        }

        [Test]
        public void TestGetInstance()
        {
            // Arrange
            PreparedSingletonFactory.Prepare(typeof(TestEntity), new TestEntity());

            // Act
            var preparedComponent1 = PreparedSingletonFactory.GetInstance<TestEntity>();
            var preparedComponent2 = PreparedSingletonFactory.GetInstance<TestEntity>();
            var singletonComponent1 = PreparedSingletonFactory.GetInstance<TestNotPrepared>();
            var singletonComponent2 = PreparedSingletonFactory.GetInstance<TestNotPrepared>();
            
            // Assert
            Assert.IsNotNull(preparedComponent1);
            Assert.IsNotNull(singletonComponent1);   // PrepareされていなくてもSingletonFactory側で生成される
            Assert.AreSame(preparedComponent1, preparedComponent2);    // singletonなので同インスタンス
            Assert.AreSame(singletonComponent1, singletonComponent2);  // singletonなので同インスタンス
        }

        [Test]
        public void TestClearPreparedComponents()
        {
            // Arrange
            PreparedSingletonFactory.Prepare(typeof(TestEntity), new TestEntity());

            // Act
            var preparedComponent1 = PreparedSingletonFactory.GetInstance<TestEntity>();
            var singletonComponent1 = PreparedSingletonFactory.GetInstance<TestNotPrepared>();
            PreparedSingletonFactory.ClearPreparedComponents();
            var preparedComponent2 = PreparedSingletonFactory.GetInstance<TestEntity>();
            var singletonComponent2 = PreparedSingletonFactory.GetInstance<TestNotPrepared>();

            // Assert
            Assert.IsNotNull(preparedComponent1);
            Assert.IsNotNull(preparedComponent2);　// Prepare分はクリアされ、SingletonFactory側でインスタンス再生成
            Assert.AreNotSame(preparedComponent1, preparedComponent2);    // 再生成されているので別インスタンス
            Assert.AreSame(singletonComponent1, singletonComponent2);     // SingletonFactory側のキャッシュは残る
        }

        private class TestEntity
        {

        }

        private class TestNotPrepared
        { }
    }
}
