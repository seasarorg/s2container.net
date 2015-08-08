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
using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
    [TestFixture]
    public class ComponentTagHandlerTest
    {
        private const string PATH = "Seasar/Tests/Framework/container/Factory/ComponentTagHandlerTest.dicon";

        [Test]
        public void TestComponent()
        {
            var container = S2ContainerFactory.Create(PATH);
            container.Init();
            Assert.IsNotNull(container.GetComponent(typeof(ArrayList)));
            Assert.IsNotNull(container.GetComponent("aaa"));
            // #.NET4.0 一度int型に変換してから比較
            Assert.AreEqual(1, Decimal.ToInt32((Decimal)container.GetComponent("bbb")));
            Assert.AreEqual(true, container.GetComponent("ccc") !=
                container.GetComponent("ccc"));
            var cd = container.GetComponentDef("ddd");
            Assert.AreEqual(ContainerConstants.AUTO_BINDING_NONE,
                cd.AutoBindingMode);
            var table = new Hashtable();
            container.InjectDependency(table, "eee");
            Assert.AreEqual("111", table["aaa"]);
            Assert.IsNotNull(container.GetComponent("fff"));
            Assert.IsNotNull(container.GetComponent("ggg"));
        }

        [Test]
        public void TestGeneric()
        {
            var container = S2ContainerFactory.Create(PATH);
            container.Init();
            Assert.IsNotNull(container.GetComponent(typeof(IList<String>)));
            Assert.IsNotNull(container.GetComponent(typeof(Dictionary<String, Int32>)));
        }
    }
}
