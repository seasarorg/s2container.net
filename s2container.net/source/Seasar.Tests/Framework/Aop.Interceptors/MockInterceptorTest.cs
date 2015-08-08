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

using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Aop.Proxy;
using MbUnit.Framework;

namespace Seasar.Tests.Framework.Aop.Interceptors
{
    [TestFixture]
    public class MockInterceptorTest
    {
        private const string MSG = "hello";
        private const string ECHO = "echo";
        private MockInterceptor _target = null;

        public static object CreateProxy(IMethodInterceptor interceptor, Type proxyType)
        {
            IAspect aspect = new AspectImpl(interceptor, null);
            return new AopProxy(proxyType, new IAspect[] { aspect }).GetTransparentProxy();
        }

        [SetUp]
        public void SetUp()
        {
            _target = new MockInterceptor(MSG);
        }

        [Test]
        public void TestInvoke()
        {
            var hello = CreateProxy(_target, typeof(Hello)) as Hello;
            Assert.AreEqual(MSG, hello.Greeting());
        }

        [Test]
        public void TestInvoke2()
        {
            _target.SetReturnValue("Greeting", MSG);
            _target.SetReturnValue("Echo", ECHO);
            var hello = CreateProxy(_target, typeof(SayHello)) as SayHello;

            var message = "hoge";

            Assert.AreEqual(ECHO, hello.Echo(message));
            Assert.IsTrue(_target.IsInvoked("Echo"));
            Assert.IsFalse(_target.IsInvoked("Greeting"));
            Assert.AreEqual(message, _target.GetArgs("Echo")[0]);
        }

        [Test]
        public void TestInvoke3()
        {
            _target.SetReturnValue("Greeting", MSG);
            _target.SetReturnValue("Echo", ECHO);
            var hello = CreateProxy(_target, typeof(SayHello)) as SayHello;

            var message = "hoge";
            Assert.AreEqual(MSG, hello.Greeting());
            Assert.AreEqual(ECHO, hello.Echo(message));
            Assert.IsTrue(_target.IsInvoked("Greeting"));
            Assert.IsTrue(_target.IsInvoked("Echo"));
        }

        [Test]
        public void TestInvokeThrowException()
        {
            Exception ex = new ApplicationException();
            _target.SetThrowable("Greeting", ex);
            var hello = CreateProxy(_target, typeof(Hello)) as Hello;
            try
            {
                hello.Greeting();
                Assert.Fail();
            }
            catch (ApplicationException exception)
            {
                Assert.AreEqual(ex, exception);
            }
        }
    }

    public interface Hello
    {
        string Greeting();
    }

    public interface SayHello : Hello
    {
        string Echo(string msg);
    }
}
