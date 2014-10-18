using NUnit.Framework;
using Seasar.Quill.Preset.Factory.Impl;
using Seasar.Quill.Preset.Scope.Impl;
using System;

namespace Seasar.Tests.Quill.Preset.Scope.Impl
{
    [TestFixture]
    public class SingletonFactoryTest
    {
        [TearDown]
        public void TearDown()
        {
            SingletonFactory.Reset();
        }

        [Test]
        public void TestGetInstance()
        {
            var component = SingletonFactory.GetInstance<Hoge>();
            Assert.IsNotNull(component);
            Assert.AreEqual(typeof(Hoge), component.GetType());
        }

        [Test]
        public void TestGetInstance_WithArgument()
        {
            var testType = typeof(Hoge);
            var component = SingletonFactory.GetInstance(testType);
            Assert.IsNotNull(component);
            Assert.AreEqual(testType, component.GetType());
        }

        [Test]
        public void TestSingleton()
        {
            var component1 = SingletonFactory.GetInstance<Hoge>();
            var component2 = SingletonFactory.GetInstance(typeof(Hoge));

            Assert.AreSame(component1, component2);
        }

        [Test]
        public void TestGetInstance_WithCallback()
        {
            var testObj = new CallBackClass();
            var component = SingletonFactory.GetInstance<Hoge>(testObj.GetHoge);

            Assert.IsNotNull(component);
            Assert.AreEqual(typeof(Hoge), component.GetType());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void TestGetInstance_ArgNull()
        {
            SingletonFactory.GetInstance(null);
        }

        [Test]
        public void TestSetValueFactory()
        {
            const char EXPECTED = 'a';
            SingletonFactory.SetValueFactory(t => new string(new char[] { EXPECTED }));

            var component = SingletonFactory.GetInstance(typeof(string));

            Assert.IsNotNull(component);
            Assert.AreEqual(EXPECTED.ToString(), component);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void TestSetValueFactory_ArgNull()
        {
            SingletonFactory.SetValueFactory(null);
        }

        [Test]
        public void TestIsCached()
        {
            Assert.IsFalse(SingletonFactory.IsCached(typeof(Hoge)));
            var component1 = SingletonFactory.GetInstance(typeof(Hoge));
            Assert.IsTrue(SingletonFactory.IsCached(typeof(Hoge)));
        }

        [Test]
        public void TestClear()
        {
            var testType = typeof(Hoge);
            Assert.IsFalse(SingletonFactory.IsCached(testType));
            var component1 = SingletonFactory.GetInstance(testType);
            Assert.IsTrue(SingletonFactory.IsCached(testType));

            SingletonFactory.Clear();
            Assert.IsFalse(SingletonFactory.IsCached(testType));
            var component2 = SingletonFactory.GetInstance(testType);
            Assert.IsTrue(SingletonFactory.IsCached(testType));

            Assert.AreNotSame(component1, component2);
        }
    }

    public class Hoge
    {
    }

    public class CallBackClass
    {
        public object GetHoge(Type targetType)
        {
            return new Hoge();
        }
    }
}
