#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.IO;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Container.Impl;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Container.Deployer
{
    [TestFixture]
    public class SingletonComponentDeployerTest
    {
        [SetUp]
        public void Setup()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [Test]
        public void TestDeployAutoAutoConstructor()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            container.Register(cd);
            container.Register(typeof(B));
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A a = (A) deployer.Deploy(typeof(A));
            Assert.AreEqual("B", a.HogeName);
            Assert.AreEqual(a, deployer.Deploy(typeof(A)));
        }

        [Test]
        public void TestDeployAutoAutoConstructorAndProperty()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            cd.AddPropertyDef(new PropertyDefImpl("Aaa", 1));
            container.Register(cd);
            container.Register(typeof(B));
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A a = (A) deployer.Deploy(typeof(A));
            Assert.AreEqual("B", a.HogeName);
            Assert.AreEqual(1, a.Aaa);
            Assert.AreEqual(a, deployer.Deploy(typeof(A)));
        }

        [Test]
        public void TestDeployAutoAutoConstructorAndProperty2()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            cd.AddPropertyDef(new PropertyDefImpl("Aaa", 1));
            container.Register(cd);
            container.Register(typeof(B));
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            Assert.AreEqual("B", a2.HogeName);
            Assert.AreEqual(1, a2.Aaa);
        }

        [Test]
        public void TestDeployAutoAutoProperty()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            container.Register(cd);
            container.Register(typeof(B));
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            Assert.AreEqual("B", a2.HogeName);
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

            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
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

            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
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

            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(2, culc.Count());
        }

        [Test]
        public void TestDeployAutoManualConstructor()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Decimal));
            cd.AddArgDef(new ArgDefImpl(123));
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            Assert.AreEqual(new Decimal(123), deployer.Deploy(typeof(decimal)));
        }

        [Test]
        public void TestDeployAutoManualProperty()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            cd.AddPropertyDef(new PropertyDefImpl("Hoge", new B()));
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            Assert.AreEqual("B", a2.HogeName);
        }

        [Test]
        public void TestDeployAutoManual()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(D));
            cd.AddArgDef(new ArgDefImpl("abc"));
            cd.AddPropertyDef(new PropertyDefImpl("Aaa", 1));
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            D d = (D) deployer.Deploy(typeof(D));
            Assert.AreEqual("abc", d.Name);
            Assert.AreEqual(1, d.Aaa);
        }

        [Test]
        public void TestGetComponentForInitMethodDef()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
            IInitMethodDef md = new InitMethodDefImpl("Add");
            md.AddArgDef(new ArgDefImpl("aaa"));
            md.AddArgDef(new ArgDefImpl("hoge"));
            cd.AddInitMethodDef(md);
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            Hashtable table = (Hashtable) deployer.Deploy(typeof(Hashtable));
            Assert.AreEqual("hoge", table["aaa"]);
        }

        [Test]
        public void TestCyclicReference()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            ComponentDefImpl cd2 = new ComponentDefImpl(typeof(C));
            container.Register(cd);
            container.Register(cd2);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            IComponentDeployer deployer2 = new SingletonComponentDeployer(cd2);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            C c = (C) deployer2.Deploy(typeof(C));
            Assert.AreEqual("C", a2.HogeName);
            Assert.AreEqual("C", c.HogeName);
        }

        [Test]
        public void TestDeployConstructor()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            container.Register(cd);
            container.Register(typeof(B));
            cd.AutoBindingMode = "constructor";
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A a = (A) deployer.Deploy(typeof(A));
            Assert.AreEqual("B", a.HogeName);
        }

        [Test]
        public void TestDeployProperty()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            container.Register(cd);
            container.Register(typeof(B));
            cd.AutoBindingMode = "property";
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            Assert.AreEqual("B", a2.HogeName);
        }

        [Test]
        public void TestDeployNoneManualConstructor()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Decimal));
            cd.AddArgDef(new ArgDefImpl(123));
            container.Register(cd);
            cd.AutoBindingMode = "none";
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            Assert.AreEqual(new Decimal(123), deployer.Deploy(typeof(decimal)));
        }

        [Test]
        public void TestDeployNoneManualProperty()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
            cd.AddPropertyDef(new PropertyDefImpl("Hoge", new B()));
            container.Register(cd);
            cd.AutoBindingMode = "none";
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            A2 a2 = (A2) deployer.Deploy(typeof(A2));
            Assert.AreEqual("B", a2.HogeName);
        }

        [Test]
        public void TestDeployNoneDefault()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(StringBuilder));
            container.Register(cd);
            cd.AutoBindingMode = "none";
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            Assert.AreEqual(string.Empty, ((StringBuilder) deployer.Deploy(typeof(StringBuilder))).ToString());
        }

        [Test]
        public void TestDeploy()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
            IDestroyMethodDef md = new DestroyMethodDefImpl("Add");
            md.AddArgDef(new ArgDefImpl("aaa"));
            md.AddArgDef(new ArgDefImpl("hoge"));
            cd.AddDestroyMethodDef(md);
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
            Hashtable table = (Hashtable) deployer.Deploy(typeof(Hashtable));
            deployer.Destroy();
            Assert.AreEqual("hoge", table["aaa"]);
        }

        [Test]
        public void TestDestroy()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(D));
            cd.AddArgDef(new ArgDefImpl("name"));
            container.Register(cd);
            container.Init();
            container.Destroy();
        }

        [Test]
        public void TestInjectDependency()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
            container.Register(cd);
            IComponentDeployer deployer = new SingletonComponentDeployer(cd);
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
            private int _aaa;

            public A(IHoge hoge)
            {
                _hoge = hoge;
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }

            public int Aaa
            {
                get { return _aaa; }
                set { _aaa = value; }
            }
        }

        public class A2 : IFoo
        {
            private IHoge _hoge;
            private int _aaa;

            public IHoge Hoge
            {
                set { _hoge = value; }
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }

            public int Aaa
            {
                get { return _aaa; }
                set { _aaa = value; }
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

        public class D : IHoge, IDisposable
        {
            private readonly string _name;
            private int _aaa;

            public D(string name)
            {
                _name = name;
            }

            public string Name
            {
                get { return _name; }
            }

            public int Aaa
            {
                get { return _aaa; }
                set { _aaa = value; }
            }

            #region IDisposable ÉÅÉìÉo

            public void Dispose()
            {
                Trace.WriteLine("Dispose!");
            }

            #endregion
        }
    }
}
