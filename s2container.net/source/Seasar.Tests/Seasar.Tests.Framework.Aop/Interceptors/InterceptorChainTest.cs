#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Tests.Framework.Aop.Interceptors
{
    [TestFixture]
	public class InterceptorChainTest
	{
        private InterceptorChain target = null;

        public static object CreateProxy(IMethodInterceptor interceptor, Type proxyType) {
            IAspect aspect = new AspectImpl(interceptor, null);
            return new AopProxy(proxyType, new IAspect[] { aspect }).GetTransparentProxy();
        }

        [SetUp]
        public void SetUp() {
            target = new InterceptorChain();
            IMethodInterceptor interceptor1 = new TestInterceptor("_A");
            IMethodInterceptor interceptor2 = new TestInterceptor("_B");
            IMethodInterceptor interceptor3 = new MockInterceptor(TestMessage4InterceptorChain.ECHO);
            target.Add(interceptor1);
            target.Add(interceptor2);
            target.Add(interceptor3);
        }

        [Test]
        public void TestInvoke() {
            GoodMorning gm = CreateProxy(target, typeof(GoodMorning)) as GoodMorning;
            string result = gm.Greeting();
            Trace.WriteLine(result);
            Assert.AreEqual(TestMessage4InterceptorChain.ECHO, result);
        }

        
	}

    public class TestMessage4InterceptorChain
    {
        public const string MSG = "hello";
        public const string ECHO = "echo";
    }

    public interface GoodMorning
    {
        string Greeting();
    }

    public class TestInterceptor : AbstractInterceptor
    {
        #region IMethodInterceptor ƒNƒ‰ƒX

        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _id = "";

        public TestInterceptor(string id) {
            _id = id;
        }

        public override object Invoke(IMethodInvocation invocation) {
            Trace.WriteLine(string.Format("{0} is called.", this.ToString() + _id));
            return invocation.Proceed();
        }

        #endregion
    }
}
