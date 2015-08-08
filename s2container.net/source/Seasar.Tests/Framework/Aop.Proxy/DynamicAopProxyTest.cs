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
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Framework.Aop;

namespace Seasar.Tests.Framework.Aop.Proxy
{
    [TestFixture]
    public class DynamicAopProxyTest : S2TestCase
    {
        HelloImpl _hello = null;
        IHello _hello2 = null;
        IHello3 _hello3 = null;
        HelloImpl2 _helloImpl2 = null;
        AutoHello _autoHello = null;
        AutoHello2 _autoHello2 = null;
        IHello4 _hello4 = null;
        ICount _count = null;
        CountImpl _countImpl = null;

        public DynamicAopProxyTest()
        {
            // log4netの初期化
            var info = new FileInfo(SystemInfo.AssemblyShortName(
                Assembly.GetExecutingAssembly()) + ".dll.config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpAspect()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAspect()
        {
            Assert.AreEqual("Hello", _hello.Greeting(), "1");
            Assert.AreEqual("Hello", _hello2.Greeting(), "2");
            Assert.AreEqual("Hello", _hello3.Greeting(), "3");
            Assert.AreEqual("Hello", _hello4.Greeting(), "4");
        }

        public void SetUpProperty()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        [Ignore("実装クラスをコンポーネントに登録した場合、同じ型のクラスだと正常に適用されるが、I/Fだとインターセプターが適用されていないオブジェクトが設定される。原因がわかるまでIgnore")]
        public void TestProperty()
        {
            Assert.AreEqual("TestProperty", _hello.Prop, "1");
            Assert.AreEqual("TestProperty", _hello2.Prop, "2");
        }

        public void SetUpSingleton()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        [Ignore("I/Fで宣言した変数と実装クラスで宣言した変数には別のインスタンスが設定されている。Quillの方では問題なく動作するため、原因がわかるまでIgnore")]
        public void TestSingleton()
        {
            _hello.Prop = "TestSingleton";
            Assert.AreEqual(_hello.Prop, _hello2.Prop);
        }

        public void SetUpArg()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestArg()
        {
            Assert.AreEqual("Hello", _helloImpl2.Greeting());
        }

        public void SetUpAutoProperty()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAutoProperty()
        {
            Assert.AreEqual("Hello", _autoHello.Greeting());
        }

        public void SetUpAutoArg()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestAutoArg()
        {
            Assert.AreEqual("Hello", _autoHello2.Greeting());
        }

        public void SetUpPerformance1()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestPerformance1()
        {
            var start = DateTime.Now;
            for (var i = 0; i < 100; ++i)
            {
                Container.GetComponent(typeof(IHello4));
            }
            var span = DateTime.Now - start;
            Debug.WriteLine(span.TotalMilliseconds + "ms");

        }

        public void SetUpPerformance2()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestPerformance2()
        {
            var hello = (IHello4) Container.GetComponent(typeof(IHello4));
            var start = DateTime.Now;
            for (var i = 0; i < 10000; ++i)
            {
                hello.Greeting();
            }
            var span = DateTime.Now - start;
            Debug.WriteLine(span.TotalMilliseconds + "ms");
        }

        public void SetUpProtectedMethod()
        {
            Include("Seasar.Tests.Framework.Aop.Proxy.DynamicProxy.dicon");
        }

        [Test, S2]
        public void TestProtectedMethod()
        {
            Assert.AreEqual(1, _count.GetCount(), "_count");
            Assert.AreEqual(1, _countImpl.GetCount(), "_countImpl");
        }

        #region Test Class & Interface

        public interface IHello
        {
            string Greeting();
            string Prop { set; get; }
        }

        public class HelloImpl : IHello
        {
            private readonly string _str = "abc";

            public virtual string Greeting()
            {
                return _str;
            }

            public string Prop { set; get; } = "default";
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
            private readonly string _message;

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
            public IHello Hello { set; get; }

            public string Greeting()
            {
                return Hello.Greeting();
            }
        }

        public class AutoHello2
        {
            private readonly IHello _hello;

            public AutoHello2(IHello hello)
            {
                _hello = hello;
            }

            public string Greeting()
            {
                return _hello.Greeting();
            }
        }

        public interface IHello4
        {
            string Greeting();
        }

        public class HelloImpl4 : IHello4
        {
            private readonly string _str = "abc";

            public virtual string Greeting()
            {
                return _str;
            }
        }

        public interface ICount
        {
            int GetCount();
        }

        public class CountImpl : ICount
        {
            private readonly int _count = 0;

            public virtual int GetCount()
            {
                return GetCount2();
            }

            protected virtual int GetCount2()
            {
                return _count;
            }
        }

        public class IncrementInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                var ret = (int) invocation.Proceed();
                return ++ret;
            }
        }

        #endregion
    }
}
