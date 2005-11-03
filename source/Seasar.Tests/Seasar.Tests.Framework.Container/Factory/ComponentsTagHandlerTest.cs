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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using NUnit.Framework;

namespace TestSeasar.Framework.Container.Factory
{
	/// <summary>
	/// ComponentsTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ComponentsTagHandlerTest
	{
		private const string PATH 
            = "Seasar/Tests/Framework/Container/Factory/ComponentsTagHandlerTest.dicon";

		[Test]
		public void TestComponent()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			Assert.AreEqual("aaa", container.Namespace);
			Assert.AreEqual(0, ((A) container.GetComponent("aaa.bbb")).Time);
			Assert.AreEqual(0, ((A) container.GetComponent("bbb")).Time);
			Assert.AreEqual(PATH, container.Path);
		}

		public class A
		{
			private long time_;

			public A()
			{
			}

			public A(long time)
			{
				time_ = time;
			}

			public long Time
			{
				get { return time_; }
				set { time_ = value; }
			}
		}
	}
}
