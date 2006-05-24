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
        IHello _hello = null;

        public DynamicAopProxyTest()
        {
            // log4netÇÃèâä˙âª
            FileInfo info = new FileInfo(SystemInfo.AssemblyShortName(
                Assembly.GetExecutingAssembly()) + ".exe.config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpProxy()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestProxy()
        {
            Assert.AreEqual("Hello", _hello.Greeting());
        }

        public void SetUpProperty()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
        }

        [Test, S2]
        public void TestProperty()
        {
            Assert.AreEqual("TestProperty", _hello.Prop);
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
}
