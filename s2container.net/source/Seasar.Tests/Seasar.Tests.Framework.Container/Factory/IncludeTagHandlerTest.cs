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
using NUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
	/// <summary>
	/// IncludeTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class IncludeTagHandlerTest
	{
		private const string PATH="Seasar/Tests/Framework/Container/Factory/IncludeTagHandlerTest.dicon";
		private const string PATH2="Seasar/Tests/Framework/Container/Factory/aaa.dicon";

		[Test]
		public void TestInclude()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			Assert.AreEqual(new A(314).Time, ((A) container.GetComponent(typeof(A))).Time);
		}

		[Test]
		public void TestInclude2()
		{
			IS2Container container = S2ContainerFactory.Create(PATH2);
			Assert.AreSame(container.GetComponent("aaa.cdecimal"),
				container.GetComponent("bbb.cdecimal"));
		}

		[Test]
		public void TestInclude3()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			IS2Container grandChild = (IS2Container) container.GetComponent("grandChild");
			IS2Container child = (IS2Container) container.GetComponent("child");
			IS2Container grandChild2 = (IS2Container) child.GetComponent("grandChild");
			Assert.AreEqual(grandChild, grandChild2);
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
