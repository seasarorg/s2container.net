#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Framework.Aop.Impl
{
    [TestFixture]
    public class PointcutImplTest
    {
        [Test]
        public void TestGetMethodNames()
        {
            PointcutImpl pointcut = new PointcutImpl(typeof(Hello2Impl));
            string[] methodNames = pointcut.MethodNames;
            Assert.AreEqual(2, methodNames.Length);
            foreach (string methodName in methodNames)
            {
                Trace.WriteLine(methodName);
            }
        }

        [Test]
        public void TestGetMethodNames2()
        {
            PointcutImpl pointcut = new PointcutImpl(typeof(Hello2));
            string[] methodNames = pointcut.MethodNames;
            Assert.AreEqual(2, methodNames.Length);
            foreach (string methodName in methodNames)
            {
                Trace.WriteLine(methodName);
            }
        }

        [Test]
        public void TestGetMethodNames3()
        {
            PointcutImpl pointcut = new PointcutImpl(typeof(Hello2Impl2));
            string[] methodNames = pointcut.MethodNames;
            Assert.AreEqual(2, methodNames.Length);
            foreach (string methodName in methodNames)
            {
                Trace.WriteLine(methodName);
            }
        }

        [Test]
        public void TestRegex()
        {
            PointcutImpl pointcut = new PointcutImpl(new string[] { "Greeting.*" });
            Assert.AreEqual(true, pointcut.IsApplied("Greeting"), "1");
            Assert.AreEqual(true, pointcut.IsApplied("Greeting2"), "2");
            Assert.AreEqual(false, pointcut.IsApplied("TestGreetingAAA"), "3");
            Assert.AreEqual(false, pointcut.IsApplied("TestGreeting"), "4");
            Assert.AreEqual(false, pointcut.IsApplied("TestRegex"), "5");
        }

        [Test]
        public void testRegex2()
        {
            PointcutImpl pointcut = new PointcutImpl(new String[] { "Find" });
            Assert.AreEqual(false, pointcut.IsApplied("GetFindEx"), "1");
            Assert.AreEqual(false, pointcut.IsApplied("FindAll"), "2");
            Assert.AreEqual(true, pointcut.IsApplied("Find"), "3");
        }

        public interface Hello
        {
            string Greeting();
        }

        public class HelloImpl : Hello
        {
            public string Greeting()
            {
                return "Hello";
            }
        }

        public class HelloInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return "Hello";
            }
        }

        public interface Hello2 : Hello
        {
            string Greeting2();
        }

        public class Hello2Impl : HelloImpl, Hello2
        {
            public string Greeting2()
            {
                return "Hello2";
            }
        }

        public class Hello2Impl2 : Hello2
        {
            public string Greeting2()
            {
                return "Hello2";
            }

            public string Greeting()
            {
                return "Hello";
            }
        }
    }
}
