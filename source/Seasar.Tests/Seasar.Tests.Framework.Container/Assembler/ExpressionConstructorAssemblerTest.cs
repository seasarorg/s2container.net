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
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using NUnit.Framework;

namespace TestSeasar.Framework.Container.Assembler
{
	/// <summary>
	/// ExpressionConstructorAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ExpressionConstructorAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(object),"obj");
			container.Register(cd);
			ComponentDefImpl cd2 = new ComponentDefImpl();
			cd2.Expression = "container.GetHashCode()";
			container.Register(cd2);
			ExpressionConstructorAssembler assembler =
				new ExpressionConstructorAssembler(cd2);
			Int32 myInt = (Int32) assembler.Assemble();
			Console.WriteLine(myInt);
			Assert.IsNotNull(myInt);
		}

		[Test]
		public void TestAssemblerForNull()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(object),"obj");
			cd.Expression = "null";
			container.Register(cd);
			ExpressionConstructorAssembler assembler = new ExpressionConstructorAssembler(cd);
			try
			{
				assembler.Assemble();
				Assert.Fail();
			}
			catch(TypeUnmatchRuntimeException ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
