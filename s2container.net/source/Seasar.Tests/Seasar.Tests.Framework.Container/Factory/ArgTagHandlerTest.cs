#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Reflection;
using NUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Xml;

namespace TestSeasar.Framework.Container.Factory
{

	/// <summary>
	/// ArgTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ArgTagHandlerTest
	{
		private const string PATH =
			"Seasar/Tests/Framework/Container/Factory/ArgTagHandlerTest.dicon";

		[Test]
		public void TestArg()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			Assert.AreEqual(new Decimal(1), container.GetComponent(typeof(Decimal)));
		}

		[Test]
		public void TestArg2()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("A", a.Name);
		}

		public class A
		{
			private string name_;

			public A(string name)
			{
				name_ = name;
			}

			public string Name
			{
				get { return name_; }
			}
		}
	}
}
