using System;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Tests.Framework.Aop.Proxy
{
    [TestFixture]
	class DynamicAopProxyTest
	{
        [Test]
        public void TestProxy()
        {
            IS2Container container = S2ContainerFactory.Create("Seasar.Tests.Framework.Aop.Proxy.proxy.dicon");
            container.Init();
            IHello hello = (IHello)container.GetComponent(typeof(IHello));
            Assert.AreEqual("Hello", hello.Greeting());
        }


	}

    public interface IHello
    {
        string Greeting();
    }

    public class HelloImpl : IHello
    {
        private string str_ = "abc";

        public HelloImpl()
        {
        }

        public virtual string Greeting()
        {
            return str_;
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
