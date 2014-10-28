using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Attr;
using Seasar.Quill.Exception;
using Seasar.Quill.Parts.Container.ImplTypeFactory.Impl;
using Seasar.Quill.Parts.Container.InstanceFactory.Impl;
using Seasar.Quill.Parts.Handler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seasar.Tests.Quill
{
    [TestFixture]
    public class QuillContainerTest : QuillTestBase
    {
        [Test]
        public void TestGetComponent_受け取り側の型と実装型が同じ()
        {
            var container = new QuillContainer();
            var actual = container.GetComponent<Hoge>();
            var expected = container.GetComponent(typeof(Hoge));

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void TestGetComponent_受け取り側の型と実装型が異なる()
        {
            var container = new QuillContainer();
            var actual = container.GetComponent<WithImplementation>();
            var expected = container.GetComponent(typeof(Hoge));

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void TestGetComponent_コンテナが違えば同じ型でも別インスタンス()
        {
            var container1 = new QuillContainer();
            var container2 = new QuillContainer();

            var component1_1 = container1.GetComponent<Hoge>();
            var component1_2 = container1.GetComponent(typeof(Hoge));
            var component2_1 = container2.GetComponent<Hoge>();
            var component2_2 = container2.GetComponent(typeof(Hoge));

            Assert.AreSame(component1_1, component1_2);
            Assert.AreSame(component2_1, component2_2);
            Assert.AreNotSame(component1_1, component2_2);
        }

        #region GetImplTypeのカスタマイズ
        [Test]
        public void TestGetComponent_CustomGetImplType()
        {
            var container = new QuillContainer();
            var typeMap = new Dictionary<Type, Type>();
            var expectedType = typeof(HogeEx);
            typeMap[typeof(WithImplementation)] = expectedType;
            var actual = container.GetComponent<WithImplementation>(callbackGetImplType: (receiptType => typeMap[receiptType]));

            Assert.AreEqual(expectedType, actual.GetType());
        }

        [Test]
        public void TestGetComponent_ExtensionGetImplType()
        {
            var container = new QuillContainer();
            var typeMap = new Dictionary<Type, Type>();
            var expectedType = typeof(HogeEx);
            typeMap[typeof(WithImplementation)] = expectedType;
            var implTypeFactory = new MappingImplTypeFactory(typeMap);
            container.ImplTypeFactory = implTypeFactory;

            var actual = container.GetComponent<WithImplementation>();

            Assert.AreEqual(expectedType, actual.GetType());
        }

        #endregion

        #region GetInstanceのカスタマイズ

        [Test]
        public void TestGetComponent_CustomGetInstance()
        {
            var container = new QuillContainer();

            var actual = container.GetComponent(typeof(Hoge), callbackGetInstance: (t => new AnotherClass()));

            Assert.AreEqual(typeof(AnotherClass), actual.GetType());
        }

        [Test]
        public void TestGetComponent_ExchangeGetInstance()
        {
            var container = new QuillContainer();
            container.InstanceFactory = new PrototypeInstanceFactory();

            var component1 = container.GetComponent<Hoge>();
            var component2 = container.GetComponent<Hoge>();

            Assert.IsNotNull(component1);
            Assert.AreNotSame(component1, component2);
        }

        #endregion

        #region ログ出力

        [Test]
        public void TestLog()
        {
            var container = new QuillContainer();
            var actual = new List<string>();
            container.Log = (msg => actual.Add(msg));

            var component = container.GetComponent<Hoge>();

            Assert.Greater(actual.Count(), 0);
            actual.ForEach(msg => Console.WriteLine(msg));
        }

        #endregion

        #region 例外発生時のテスト

        [Test]
        [ExpectedException(typeof(QuillApplicationException))]
        public void TestHandleQuillApplicationException()
        {
            var container = new QuillContainer();
            var component = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<QuillApplicationException>(t)));
        }

        [Test]
        [ExpectedException(typeof(System.Exception))]
        public void TestHandleSystemException()
        {
            var container = new QuillContainer();
            var component = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<System.Exception>(t)));
        }

        [Test]
        public void TestCustomHandleQuillApplicationException()
        {
            var container = new QuillContainer();
            var actual = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<QuillApplicationException>(t)),
                handleQuillApplicationException: (e => typeof(QuillContainer).Name));
            Assert.AreEqual(typeof(QuillContainer).Name, actual);
        }

        [Test]
        public void TestCustomHandleSystemException()
        {
            var container = new QuillContainer();
            var actual = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<System.Exception>(t)),
                handleSystemException: (e =>typeof(QuillContainer).Name));
            Assert.AreEqual(typeof(QuillContainer).Name, actual);
        }

        [Test]
        public void TestExchangeHandleQuillApplicationException()
        {
            var container = new QuillContainer();
            container.QuillApplicationExceptionHandler = new ExQAppExceptionHandler();
            var actual = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<QuillApplicationException>(t)));
            Assert.AreEqual(typeof(ExQAppExceptionHandler).Name, actual);
        }

        [Test]
        public void TestExchangeHandleSystemnException()
        {
            var container = new QuillContainer();
            container.SystemExceptionHandler = new ExSysExceptionHandler();
            var actual = container.GetComponent(typeof(Hoge), callbackGetImplType: (t => GetImplTypeWithException<System.Exception>(t)));
            Assert.AreEqual(typeof(ExSysExceptionHandler).Name, actual);
        }
        #endregion

        #region テスト用補助メソッド
        private Type GetImplTypeWithException<EXCEPTION>(Type receiptType) where EXCEPTION : System.Exception, new()
        {
            throw new EXCEPTION();
        }
        #endregion

        #region テスト用補助クラス
        private class Hoge : WithImplementation
        { }

        private class HogeEx : WithImplementation
        { }

        private class AnotherClass
        { }

        private class ExQAppExceptionHandler : IQuillApplicationExceptionHandler
        {
            public object Handle(QuillApplicationException ex)
            {
                return typeof(ExQAppExceptionHandler).Name;
            }
        }

        private class ExSysExceptionHandler : ISystemExceptionHandler
        {
            public object Handle(Exception ex)
            {
                return typeof(ExSysExceptionHandler).Name;
            }
        }

        [Implementation(ImplType = typeof(Hoge))]
        private class WithImplementation
        { }

        #endregion
    }
}
