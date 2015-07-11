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
using System.Collections;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Deployer
{
    [TestFixture]
    public class PrototypeComponentDeployerTest
    {
        [Test]
        public void TestDeployAutoAutoConstructor()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            container.Register(cd);
            container.Register(typeof(B));
            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            A a = (A) deployer.Deploy(typeof(A));
            Assert.AreEqual("B", a.HogeName);
            Assert.AreEqual(false, a == deployer.Deploy(typeof(A)));
        }

        [Test]
        public void TestCyclicReference()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            ComponentDefImpl cd2 = new ComponentDefImpl(typeof(C));
            container.Register(cd);
            container.Register(cd2);
            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            IComponentDeployer deployer2 = new PrototypeComponentDeployer(cd2);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            C c = (C) deployer2.Deploy(typeof(C));
            Assert.AreEqual("C", a2.HogeName);
            Assert.AreEqual("C", c.HogeName);
        }

        [Test]
        public void TestInjectDependency()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl();
            container.Register(cd);
            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            try
            {
                deployer.InjectDependency(new Hashtable());
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Trace.WriteLine(ex);
            }
        }

        [Test]
        public void TestDeployAspect1()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl1));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = container;
            cd.AddAspeceDef(ad);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            container.Register(plusOneCd);
            container.Register(cd);

            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            ICulc culc = (ICulc) deployer.Deploy(typeof(ICulc));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(1, culc.Count());
        }

        [Test]
        public void TestDeployAspect2()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = container;
            cd.AddAspeceDef(ad);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            container.Register(plusOneCd);
            container.Register(cd);

            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(1, culc.Count());
        }

        [Test]
        public void TestDeployAspect3()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = container;

            IAspectDef ad2 = new AspectDefImpl();
            ad2.Expression = "plusOne";
            ad2.Container = container;

            cd.AddAspeceDef(ad);
            cd.AddAspeceDef(ad2);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            container.Register(plusOneCd);
            container.Register(cd);

            IComponentDeployer deployer = new PrototypeComponentDeployer(cd);
            CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(2, culc.Count());
        }

        public class PlusOneInterceptor : IMethodInterceptor
        {
            public static int Count = 0;
            public object Invoke(IMethodInvocation invocation)
            {
                ++Count;
                invocation.Proceed();
                return Count;
            }
        }

        public interface ICulc
        {
            int Count();
        }

        public class CulcImpl1 : ICulc
        {
            public int Count()
            {
                return 0;
            }
        }

        public class CulcImpl2 : MarshalByRefObject, ICulc
        {
            public int Count()
            {
                return 0;
            }
        }

        public interface IFoo
        {
            string HogeName { get; }
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

        public class A2 : IFoo
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
            private IFoo _foo;

            public IFoo Foo
            {
                set { _foo = value; }
            }

            public string Name
            {
                get { return "C"; }
            }

            public string HogeName
            {
                get { return _foo.HogeName; }
            }
        }
    }
}
