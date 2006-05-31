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
	class DynamicAopProxyTest : S2TestCase
	{
        HelloImpl _hello = null;
        IHello _hello2 = null;
        IHello3 _hello3 = null;
        HelloImpl2 _helloImpl2 = null;
        AutoHello _autoHello = null;
        AutoHello2 _autoHello2 = null;

        public DynamicAopProxyTest()
        {
            // log4netÇÃèâä˙âª
            FileInfo info = new FileInfo(SystemInfo.AssemblyShortName(
                Assembly.GetExecutingAssembly()) + ".exe.config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpAspect()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestAspect()
        {
            Assert.AreEqual("Hello", _hello.Greeting(), "1");
            Assert.AreEqual("Hello", _hello2.Greeting(), "2");
            Assert.AreEqual("Hello", _hello3.Greeting(), "3");
        }

        public void SetUpProperty()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestProperty()
        {
            Assert.AreEqual("TestProperty", _hello.Prop, "1");
            Assert.AreEqual("TestProperty", _hello2.Prop, "2");
        }

        public void SetUpSingleton()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestSingleton()
        {
            _hello.Prop = "TestSingleton";
            Assert.AreEqual(_hello.Prop, _hello2.Prop);
        }

        public void SetUpArg()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestArg()
        {
            Assert.AreEqual("Hello", _helloImpl2.Greeting());
        }

        public void SetUpAutoProperty()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestAutoProperty()
        {
            Assert.AreEqual("Hello", _autoHello.Greeting());
        }

        public void SetUpAutoArg()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestAutoArg()
        {
            Assert.AreEqual("Hello", _autoHello2.Greeting());
        }
	}

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
}
