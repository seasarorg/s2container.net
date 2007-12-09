#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Web;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Deployer
{
    [TestFixture]
    public class RequestComponentDeployerTest
    {
        private IS2Container _container;

        [SetUp]
        public void SetUp()
        {
            HttpRequest request = new HttpRequest("hello.html", "http://localhost/hello.html", string.Empty);
            HttpResponse response = new HttpResponse(null);
            HttpContext.Current = new HttpContext(request, response);
            _container = new S2ContainerImpl();
            _container.HttpContext = HttpContext.Current;
        }

        [Test]
        public void TestDeployAutoAutoConstructor()
        {
            IComponentDef cd = new ComponentDefImpl(typeof(Foo), "foo");
            _container.Register(cd);
            IComponentDeployer deployer = new RequestComponentDeployer(cd);
            Foo foo = (Foo) deployer.Deploy(typeof(Foo));
            Assert.AreSame(foo, HttpContext.Current.Items["foo"]);
            Assert.AreSame(foo, deployer.Deploy(typeof(Foo)));
        }

        [Test]
        public void TestDeployAspect1()
        {
            _container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl1));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = _container;
            cd.AddAspeceDef(ad);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            _container.Register(plusOneCd);
            _container.Register(cd);

            IComponentDeployer deployer = new RequestComponentDeployer(cd);
            ICulc culc = (ICulc) deployer.Deploy(typeof(ICulc));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(1, culc.Count());
        }

        [Test]
        public void TestDeployAspect2()
        {
            _container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = _container;
            cd.AddAspeceDef(ad);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            _container.Register(plusOneCd);
            _container.Register(cd);

            IComponentDeployer deployer = new RequestComponentDeployer(cd);
            CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(1, culc.Count());
        }

        [Test]
        public void TestDeployAspect3()
        {
            _container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = _container;

            IAspectDef ad2 = new AspectDefImpl();
            ad2.Expression = "plusOne";
            ad2.Container = _container;

            cd.AddAspeceDef(ad);
            cd.AddAspeceDef(ad2);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            _container.Register(plusOneCd);
            _container.Register(cd);

            IComponentDeployer deployer = new RequestComponentDeployer(cd);
            CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(2, culc.Count());
        }

        [Test]
        public void TestDeployAspect4()
        {
            _container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl1));

            IAspectDef ad = new AspectDefImpl();
            ad.Expression = "plusOne";
            ad.Container = _container;
            cd.AddAspeceDef(ad);
            ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
            _container.Register(plusOneCd);
            _container.Register(cd);

            IComponentDeployer deployer = new RequestComponentDeployer(cd);
            ICulc culc = (ICulc) deployer.Deploy(typeof(ICulc));
            PlusOneInterceptor.Count = 0;
            Assert.AreEqual(1, culc.Count());

            ICulc culc2 = (ICulc) deployer.Deploy(typeof(ICulc));
            Assert.AreEqual(2, culc2.Count());
        }

        public class Foo
        {
            private string _message;

            public string Message
            {
                set { _message = value; }
                get { return _message; }
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
    }
}
