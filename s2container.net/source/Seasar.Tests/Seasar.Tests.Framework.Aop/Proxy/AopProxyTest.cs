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
using NUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Tests.Framework.Aop.Proxy
{
	/// <summary>
	/// AopProxyTest の概要の説明です。
	/// </summary>
	[TestFixture]
	public class AopProxyTest
	{
		public AopProxyTest()
		{
		}

		[Test]
		public void TestInterface()
		{
			IPointcut pointcut = new PointcutImpl(new string[] { "Greeting" });
			IAspect aspect = new AspectImpl(new HelloInterceptor(),pointcut);
			AopProxy aopProxy = new AopProxy(typeof(IHello),new IAspect[] { aspect } );
			IHello proxy = (IHello) aopProxy.GetTransparentProxy();
			Assert.AreEqual("Hello",proxy.Greeting());
		}

		[Test]
		public void TestCreateForArgs()
		{
			IAspect aspect = new AspectImpl(new TraceInterceptor());
			AopProxy aopProxy = new AopProxy(typeof(HelloImpl), new IAspect[] { aspect });
			IHello proxy = (IHello) aopProxy.Create(new Type[] { typeof(string) },
				new object[] { "Hello" });
			Assert.AreEqual("Hello",proxy.Greeting());
			Console.WriteLine(proxy.GetHashCode());
		}

		[Test]
		public void TestEquals()
		{
			IPointcut pointcut = new PointcutImpl(new string[] { "Greeting" });
			IAspect aspect = new AspectImpl(new HelloInterceptor(),pointcut);
			AopProxy aopProxy = new AopProxy(typeof(IHello), new IAspect[] { aspect });
			IHello proxy = (IHello) aopProxy.Create();
			
			//Assert.AreEqual(true,proxy.Equals(proxy));　これは駄目
			Assert.AreEqual(true,object.Equals(proxy,proxy));
			Assert.AreEqual(false,object.Equals(proxy,null));
			Assert.AreEqual(false,object.Equals(proxy,"hoge"));
		}

		/// <summary>
		/// AopProxyで
		/// System.ObjectのFieldGetterを実行しようとして、
		/// エラーが発生しています。
		/// </summary>
//		[Test]
//		public void TestSerialize()
//		{
//			IPointcut pointcut = new PointcutImpl(new string[] { "Greeting" });
//			IAspect aspect = new AspectImpl(new HelloInterceptor(),pointcut);
//			AopProxy aopProxy = new AopProxy(typeof(HelloImpl),
//				new IAspect[] { aspect });
//			HelloImpl proxy = (HelloImpl) aopProxy.Create(new Type[] { typeof(string) },
//				new object[] {"Hello"});
//
//			IHello copy = (IHello) SerializeUtil.Serialize(proxy);
//			Assert.AreEqual("Hello",copy.Greeting());
//		}

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
	}
}
