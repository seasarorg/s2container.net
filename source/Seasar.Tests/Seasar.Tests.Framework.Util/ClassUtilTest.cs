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
using NUnit.Framework;
using System.Reflection;
using Seasar.Framework.Util;
using Seasar.Framework.Exceptions;

namespace TestSeasar.Framework.Util
{
	/// <summary>
	/// ClassUtilTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ClassUtilTest
	{
		[Test]
		public void TestGetConstructorInfo()
		{
			try
			{
				ConstructorInfo constructor = ClassUtil.GetConstructorInfo(
					typeof(A),Type.EmptyTypes);
				Assert.Fail();
			}
			catch(NoSuchConstructorRuntimeException ex)
			{
				Console.WriteLine(ex.Message);
			}
			Type[] types = new Type[] { typeof(string) };
			ConstructorInfo constructor2 = ClassUtil.GetConstructorInfo(
				typeof(A), types);
			Assert.IsNotNull(constructor2);
		}

		[Test]
		public void TestForName()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			Assert.AreEqual(typeof(A), ClassUtil.ForName(
				"TestSeasar.Framework.Util.ClassUtilTest+A",
				new Assembly[] {asm}));
		}

		[Test]
		public void TestNewInstance()
		{
			Assert.IsNotNull(ClassUtil.NewInstance(typeof(B)));
		}

		[Test]
		public void TestNewInstance2()
		{
			Assert.IsNotNull(ClassUtil.NewInstance(
				"TestSeasar.Framework.Util.ClassUtilTest+B",
				"Seasar.Tests.dll"));
		}

		public class A
		{
			private string abc_;

			public A(string abc)
			{
				abc_ = abc;
			}
		}

		public class B
		{
			public B()
			{
			}
		}
	}
}
