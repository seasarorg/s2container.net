#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

#if !NET_1_1
using System.Collections.Generic;
#endif

namespace Seasar.Tests.Framework.Container.Factory
{
	/// <summary>
	/// ComponentTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ComponentTagHandlerTest
	{
#if NET_1_1
		private const string PATH = "Seasar/Tests/Framework/Container/Factory/ComponentTagHandlerTest.2003.dicon";
#else
        private const string PATH = "Seasar/Tests/Framework/Container/Factory/ComponentTagHandlerTest.dicon";
#endif

		[Test]
		public void TestComponent()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			container.Init();
			Assert.IsNotNull(container.GetComponent(typeof(ArrayList)));
			Assert.IsNotNull(container.GetComponent("aaa"));
			Assert.AreEqual(1, container.GetComponent("bbb"));
			Assert.AreEqual(true, container.GetComponent("ccc") != 
				container.GetComponent("ccc"));
			IComponentDef cd = container.GetComponentDef("ddd");
			Assert.AreEqual(ContainerConstants.AUTO_BINDING_NONE,
				cd.AutoBindingMode);
			Hashtable table = new Hashtable();
			container.InjectDependency(table, "eee");
			Assert.AreEqual("111", table["aaa"]);
			Assert.IsNotNull(container.GetComponent("fff"));
			Assert.IsNotNull(container.GetComponent("ggg"));
		}

#if !NET_1_1

        [Test]
        public void TestGeneric()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            container.Init();
            Assert.IsNotNull(container.GetComponent(typeof(IList<String>)));
            Assert.IsNotNull(container.GetComponent(typeof(Dictionary<String, Int32>)));
        }

#endif
		
	}
}
