using NUnit.Framework;
using Seasar.Quill.Parts.Container.InstanceFactory.Impl;
using System;

namespace Seasar.Tests.Quill.Preset.Scope
{
    [TestFixture]
    public class SingletonInstanceFactoryTest : QuillTestBase
    {
        [Test]
        public void TestGetInstance()
        {
            var factory = new SingletonInstanceFactory();
            var component = factory.GetInstance(typeof(Hoge));
            Assert.IsNotNull(component);
            Assert.AreEqual(typeof(Hoge), component.GetType());
        }

        [Test]
        public void TestGetInstance_WithArgument()
        {
            var testType = typeof(Hoge);
            var factory = new SingletonInstanceFactory();
            var component = factory.GetInstance(testType);
            Assert.IsNotNull(component);
            Assert.AreEqual(testType, component.GetType());
        }

        [Test]
        public void TestSingleton()
        {
            var factory = new SingletonInstanceFactory();
            var component1 = factory.GetInstance(typeof(Hoge));
            var component2 = factory.GetInstance(typeof(Hoge));

            Assert.AreSame(component1, component2);
        }

        [Test]
        public void TestGetInstance_WithCallback()
        {
            var testObj = new CallBackClass();
            var factory = new SingletonInstanceFactory(testObj.GetHoge);
            var component = factory.GetInstance(typeof(Hoge));

            Assert.IsNotNull(component);
            Assert.AreEqual(typeof(Hoge), component.GetType());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void TestGetInstance_ArgNull()
        {
            var factory = new SingletonInstanceFactory();
            factory.GetInstance(null);
        }

        [Test]
        public void TestSetValueFactory()
        {
            const char EXPECTED = 'a';
            var factory = new SingletonInstanceFactory(t => new string(new char[] { EXPECTED }));

            var component = factory.GetInstance(typeof(string));

            Assert.IsNotNull(component);
            Assert.AreEqual(EXPECTED.ToString(), component);
        }

        [Test]
        public void TestDispose()
        {
            var testType = typeof(Hoge);
            var factory = new SingletonInstanceFactory();
            var component1 = factory.GetInstance(testType);

            factory.Dispose();
            var component2 = factory.GetInstance(testType);

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
