using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Framework.Aop.Proxy
{
    [TestFixture]
	public class DynamicAopProxyTest : S2TestCase
	{
        HelloImpl _hello = null;
        IHello _hello2 = null;
        IHello3 _hello3 = null;
        HelloImpl2 _helloImpl2 = null;
        AutoHello _autoHello = null;
        AutoHello2 _autoHello2 = null;
        IHello4 _hello4 = null;
        ICount _count = null;
        CountImpl _countImpl = null;

        public DynamicAopProxyTest()
        {
            // log4netÇÃèâä˙âª
            FileInfo info = new FileInfo(SystemInfo.AssemblyShortName(
                Assembly.GetExecutingAssembly()) + ".dll.config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpAspect()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAspect()
        {
            Assert.AreEqual("Hello", _hello.Greeting(), "1");
            Assert.AreEqual("Hello", _hello2.Greeting(), "2");
            Assert.AreEqual("Hello", _hello3.Greeting(), "3");
            Assert.AreEqual("Hello", _hello4.Greeting(), "4");
        }

        public void SetUpProperty()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestProperty()
        {
            Assert.AreEqual("TestProperty", _hello.Prop, "1");
            Assert.AreEqual("TestProperty", _hello2.Prop, "2");
        }

        public void SetUpSingleton()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestSingleton()
        {
            _hello.Prop = "TestSingleton";
            Assert.AreEqual(_hello.Prop, _hello2.Prop);
        }

        public void SetUpArg()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestArg()
        {
            Assert.AreEqual("Hello", _helloImpl2.Greeting());
        }

        public void SetUpAutoProperty()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAutoProperty()
        {
            Assert.AreEqual("Hello", _autoHello.Greeting());
        }

        public void SetUpAutoArg()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAutoArg()
        {
            Assert.AreEqual("Hello", _autoHello2.Greeting());
        }

        public void SetUpPerformance1()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestPerformance1()
        {
            DateTime start = DateTime.Now;
            for (int i = 0; i < 100; ++i)
            {
                this.Container.GetComponent(typeof(IHello4));
            }
            TimeSpan span = DateTime.Now - start;
            System.Diagnostics.Debug.WriteLine(span.TotalMilliseconds + "ms");
            
        }

        public void SetUpPerformance2()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestPerformance2()
        {
            IHello4 hello = (IHello4)this.Container.GetComponent(typeof(IHello4));
            DateTime start = DateTime.Now;
            for (int i = 0; i < 10000; ++i)
            {
                hello.Greeting();
            }
            TimeSpan span = DateTime.Now - start;
            System.Diagnostics.Debug.WriteLine(span.TotalMilliseconds + "ms");
        }

        public void SetUpProtectedMethod()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestProtectedMethod()
        {
            Assert.AreEqual(1, _count.GetCount(), "_count");
            Assert.AreEqual(1, _countImpl.GetCount(), "_countImpl");
        }

        #region Test Class & Interface

        public interface IHello
        {
            string Greeting();
            string Prop { set; get; }
        }

        public class HelloImpl : IHello
        {
            private string _str = "abc";
            private string _prop = "default";
            public HelloImpl()
            {
            }

            public virtual string Greeting()
            {
                return _str;
            }

            public string Prop
            {
                set { _prop = value; }
                get { return _prop; }
            }
        }

        public class HelloInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return "Hello";
            }
        }

        public interface IHello3
        {
            string Greeting();
        }

        public class HelloImpl2
        {
            private string _message;

            public HelloImpl2(string message)
            {
                _message = message;
            }

            public string Greeting()
            {
                return _message;
            }
        }

        public class AutoHello
        {
            private IHello _hello;

            public IHello Hello
            {
                set { _hello = value; }
                get { return _hello; }
            }

            public string Greeting()
            {
                return _hello.Greeting();
            }
        }

        public class AutoHello2
        {
            private IHello _hello;

            public AutoHello2(IHello hello)
            {
                _hello = hello;
            }

            public string Greeting()
            {
                return _hello.Greeting();
            }
        }

        public interface IHello4
        {
            string Greeting();
        }

        public class HelloImpl4 : IHello4
        {
            private string _str = "abc";

            public string Greeting()
            {
                return _str;
            }
        }

        public interface ICount
        {
            int GetCount();
        }

        public class CountImpl : ICount
        {
            private int _count = 0;

            public virtual int GetCount()
            {
                return GetCount2();
            }

            protected virtual int GetCount2()
            {
                return _count;
            }
        }

        public class IncrementInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                int ret = (int) invocation.Proceed();
                return ++ret;
            }
        }

        #endregion

    }

}
