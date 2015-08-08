#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Tests.Framework.Aop.Interceptors
{
    [TestFixture]
    public class InterceptorChainTest
    {
        private InterceptorChain _target = null;

        public static object CreateProxy(IMethodInterceptor interceptor, Type proxyType)
        {
            IAspect aspect = new AspectImpl(interceptor, null);
            return new AopProxy(proxyType, new IAspect[] { aspect }).GetTransparentProxy();
        }

        [SetUp]
        public void SetUp()
        {
            _target = new InterceptorChain();
            IMethodInterceptor interceptor1 = new TestInterceptor("_A");
            IMethodInterceptor interceptor2 = new TestInterceptor("_B");
            IMethodInterceptor interceptor3 = new MockInterceptor(TestMessage4InterceptorChain.ECHO);
            _target.Add(interceptor1);
            _target.Add(interceptor2);
            _target.Add(interceptor3);
        }

        [Test]
        public void TestInvoke()
        {
            var gm = CreateProxy(_target, typeof(GoodMorning)) as GoodMorning;
            var result = gm.Greeting();
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

        private string _id = string.Empty;

        public TestInterceptor(string id)
        {
            _id = id;
        }

        public override object Invoke(IMethodInvocation invocation)
        {
            Trace.WriteLine(string.Format("{0} is called.", ToString() + _id));
            return invocation.Proceed();
        }

        #endregion
    }
}
