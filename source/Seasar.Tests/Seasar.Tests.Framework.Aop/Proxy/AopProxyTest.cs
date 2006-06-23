#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Framework.Aop.Proxy
{
	[TestFixture]
	public class AopProxyTest : S2TestCase
	{

		public AopProxyTest()
		{
		}

		[Test, S2]
		public void TestInterface()
		{
			IPointcut pointcut = new PointcutImpl(new string[] { "Greeting" });
			IAspect aspect = new AspectImpl(new HelloInterceptor(),pointcut);
			AopProxy aopProxy = new AopProxy(typeof(IHello),new IAspect[] { aspect } );
			IHello proxy = (IHello) aopProxy.GetTransparentProxy();
			Assert.AreEqual("Hello",proxy.Greeting());
		}

		[Test, S2]
		public void TestCreateForArgs()
		{
			IAspect aspect = new AspectImpl(new TraceInterceptor());
			AopProxy aopProxy = new AopProxy(typeof(HelloImpl), new IAspect[] { aspect });
			IHello proxy = (IHello) aopProxy.Create(new Type[] { typeof(string) },
				new object[] { "Hello" });
			Assert.AreEqual("Hello",proxy.Greeting());
			Console.WriteLine(proxy.GetHashCode());
		}

		[Test, S2]
		public void TestEquals()
		{
			IPointcut pointcut = new PointcutImpl(new string[] { "Greeting" });
			IAspect aspect = new AspectImpl(new HelloInterceptor(),pointcut);
			AopProxy aopProxy = new AopProxy(typeof(IHello), new IAspect[] { aspect });
			IHello proxy = (IHello) aopProxy.Create();
			
			//Assert.AreEqual(true,proxy.Equals(proxy));Å@Ç±ÇÍÇÕë ñ⁄
			Assert.AreEqual(true,object.Equals(proxy,proxy));
			Assert.AreEqual(false,object.Equals(proxy,null));
			Assert.AreEqual(false,object.Equals(proxy,"hoge"));
		}

        public void SetUpPerformance1()
        {
            this.Include("Seasar.Tests.Framework.Aop.Proxy.AopProxy.dicon");
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
            this.Include("Seasar.Tests.Framework.Aop.Proxy.AopProxy.dicon");
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

		public class TestInvocation : IMethodInterceptor
		{
			internal bool invoked_ = false;

			public object Invoke(IMethodInvocation invocation)
			{
				invoked_ = true;
				return invocation.Proceed();
			}
		}

		public class MyInvocation : IMethodInterceptor
		{
			public object Invoke(IMethodInvocation invocation)
			{
				return invocation.Proceed();
			}
		}

		public interface IHello
		{
			string Greeting();
		}

		[Serializable()]
		public class HelloImpl : MarshalByRefObject, IHello
		{
			private string str_;
			public HelloImpl(string str)
			{
				str_ = str;
			}

			public string Greeting()
			{
				return str_;
			}

		}

		public class Hello2Impl : IHello
		{
			public string Greeting()
			{
				return "Hello2";
			}
		}

		public class HelloImpl3 : IHello
		{
			public string Greeting()
			{
				return "hoge";
			}
		}

		public class HelloInterceptor : IMethodInterceptor
		{
			public object Invoke(IMethodInvocation invocation)
			{
				return "Hello";
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

	}
}
