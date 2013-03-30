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

using MbUnit.Framework;
using Seasar.Framework.Container.AutoRegister;
using Seasar.Framework.Aop;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.AutoRegister
{
    [TestFixture]
    public class AspectAutoRegisterTest
    {
        [Test]
        public void TestRegisterAll()
        {
            IS2Container container = new S2ContainerImpl();
            IComponentDef cd = new ComponentDefImpl(typeof(BarAspectAutoRegisterLogic));
            container.Register(cd);
            AspectAutoRegister register = new AspectAutoRegister();
            register.Container = container;
            register.AddClassPattern("Seasar.Tests.Framework.Container.AutoRegister", 
                ".*AspectAutoRegisterLogic");
            register.Interceptor = new GreetInterceptor();
            register.RegisterAll();

            Assert.AreEqual(1, cd.AspectDefSize, "1");

            IAspectDef ad = cd.GetAspectDef(0);
            Assert.AreEqual(typeof(GreetInterceptor), ad.Aspect.MethodInterceptor.GetType(), "2");

            IBar bar = (IBar) container.GetComponent(typeof(IBar));

            Assert.AreEqual("Hello", bar.Greet(), "3");
        }

        private interface IBar
        {
            string Greet();
        }

        private class BarAspectAutoRegisterLogic : IBar
        {
            public string Greet()
            {
                return null;
            }
        }

        private class GreetInterceptor : IMethodInterceptor
        {
            #region IMethodInterceptor ÉÅÉìÉo

            public object Invoke(IMethodInvocation invocation)
            {
                return "Hello";
            }

            #endregion
        }
    }
}
