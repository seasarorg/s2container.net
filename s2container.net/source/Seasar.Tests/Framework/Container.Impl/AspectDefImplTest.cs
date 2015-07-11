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

using MbUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Impl
{
    [TestFixture]
    public class AspectDefImplTest
    {
        [Test]
        public void TestSetExpression()
        {
            IS2Container container = new S2ContainerImpl();
            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "traceAdvice";
            ad.Container = container;
            ComponentDefImpl cd = new ComponentDefImpl(typeof(TraceInterceptor), "traceAdvice");
            container.Register(cd);
            Assert.AreEqual(typeof(TraceInterceptor), ad.Aspect.MethodInterceptor.GetType());
        }

        public class A
        {
            private readonly IHoge _hoge;

            public A(IHoge hoge)
            {
                _hoge = hoge;
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }
        }

        public class A2
        {
            private IHoge _hoge;

            public IHoge Hoge
            {
                set { _hoge = value; }
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }
        }

        public interface IHoge
        {
            string Name { get; }
        }

        public class B : IHoge
        {
            public string Name
            {
                get { return "B"; }
            }
        }

        public class C : IHoge
        {
            private A2 _a2;

            public A2 A2
            {
                set { _a2 = value; }
            }

            public string Name
            {
                get { return "C"; }
            }

            public string HogeName
            {
                get { return _a2.HogeName; }
            }
        }
    }
}
