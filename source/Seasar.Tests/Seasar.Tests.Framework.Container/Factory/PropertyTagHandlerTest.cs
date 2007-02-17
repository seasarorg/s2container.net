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
using System.Text;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
	/// <summary>
	/// PropertyTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class PropertyTagHandlerTest
	{
		private const string PATH 
            = "Seasar/Tests/Framework/Container/Factory/PropertyTagHandlerTest.dicon";

		[Test]
		public void TestProperty()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			Assert.AreEqual(515, ((StringBuilder) container.GetComponent(typeof(StringBuilder))).Capacity);
		}

		[Test]
		public void TestWithArg()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			StringBuilder sb = (StringBuilder) container.GetComponent(typeof(StringBuilder));
			Assert.AreEqual(515, sb.Capacity);
			Assert.AreEqual("test", sb.ToString());
		}
	}
}
