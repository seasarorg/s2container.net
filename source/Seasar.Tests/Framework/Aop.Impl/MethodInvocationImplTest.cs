#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Tests.Framework.Aop.Impl
{
    [TestFixture]
    public class MethodInvocationImplTest
    {
        [Test]
        public void TestProceed()
        {
            TestInterceptor interceptor = new TestInterceptor();
            TestInterceptor interceptor2 = new TestInterceptor();
            IPointcut pointcut = new PointcutImpl(new string[] { "Foo" });
            IAspect aspect = new AspectImpl(interceptor, pointcut);
            IAspect aspect2 = new AspectImpl(interceptor2, pointcut);

            Hoge proxy = new HogeImpl();
            AopProxy aopProxy = new AopProxy(typeof(Hoge), new IAspect[] { aspect, aspect2 }, null, proxy);
            proxy = (Hoge) aopProxy.GetTransparentProxy();
            Trace.WriteLine(proxy.Foo());
            Assert.AreEqual(true, interceptor.invoked_);
            Assert.AreEqual(true, interceptor2.invoked_);
        }

        [Test]
        public void TestProceedForAbstractMethod()
        {
            HogeInterceptor interceptor = new HogeInterceptor();
            IAspect aspect = new AspectImpl(interceptor);
            Hoge proxy = new HogeImpl();
            AopProxy aopProxy = new AopProxy(typeof(Hoge), new IAspect[] { aspect }, null, proxy);
            proxy = (Hoge) aopProxy.GetTransparentProxy();
            Assert.AreEqual("Hello", proxy.Foo());
        }

        public class TestInterceptor : IMethodInterceptor
        {
            internal bool invoked_ = false;

            public object Invoke(IMethodInvocation invocation)
            {
                invoked_ = true;
                Trace.WriteLine("before");
                object ret = invocation.Proceed();
                Trace.WriteLine("after");
                return ret;
            }
        }

        public interface Hoge
        {
            string Foo();
        }

        public class HogeImpl : Hoge
        {
            #region Hoge ÉÅÉìÉo

            public string Foo()
            {
                // TODO:  HogeImpl.Foo é¿ëïÇí«â¡ÇµÇ‹Ç∑ÅB
                Trace.WriteLine("Foo");
                return "hogehoge";
            }

            #endregion
        }

        public class HogeInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return "Hello";
            }
        }
    }
}
