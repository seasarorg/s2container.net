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

using MbUnit.Framework;
using Seasar.Framework.Container.AutoRegister;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container;

namespace Seasar.Tests.Framework.Container.AutoRegister
{
    [TestFixture]
    public class AbstractComponentAutoRegisterTest
    {
        [Test]
        public void TestRegister()
        {
            AbstractComponentAutoRegister register 
                = new TestAbstractComponentAutoRegister();
            register.Container = new S2ContainerImpl();
            register.Register(typeof(Hoge));

            IComponentDef cd = register.Container.GetComponentDef(typeof(Hoge));

            Assert.AreEqual("Hoge", cd.ComponentName, "1");
            Assert.AreEqual(ContainerConstants.INSTANCE_SINGLETON, cd.InstanceMode, "2");
            Assert.AreEqual(ContainerConstants.AUTO_BINDING_AUTO, cd.AutoBindingMode, "3");
        }

        [Test]
        public void TestProcessType()
        {
            AbstractComponentAutoRegister register
                = new TestAbstractComponentAutoRegister();
            register.Container = new S2ContainerImpl();

            register.ProcessType(typeof(Hoge));
            Assert.AreEqual(0, register.Container.ComponentDefSize, "1");

            register.AddClassPattern("Seasar.Tests.Framework.Container.AutoRegister", "Hog.*");
            register.ProcessType(typeof(Hoge));
            register.ProcessType(typeof(Hoge2));
            Assert.AreEqual(2, register.Container.ComponentDefSize, "2");

            register.Container = new S2ContainerImpl();
            register.AddClassPattern("Seasar.Tests.Framework.Container.AutoRegister", "Hog.*");
            register.AddIgnoreClassPattern("Seasar.Tests.Framework.Container.AutoRegister", "Hoge2");
            register.ProcessType(typeof(Hoge));
            register.ProcessType(typeof(Hoge2));
            Assert.AreEqual(1, register.Container.ComponentDefSize, "2");
        }

        private class TestAbstractComponentAutoRegister : AbstractComponentAutoRegister
        {
            public override void RegisterAll()
            {
                return;
            }
        }

        private class Hoge
        {
        }

        private class Hoge2
        {
        }
    }
}
