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
using System.Collections;
using System.Diagnostics;
using Seasar.Framework.Beans;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using MbUnit.Framework;

namespace Seasar.Tests.Framework.Container.Assembler
{
	/// <summary>
	/// DefaultDestroyMethodAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class DefaultDestroyMethodAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			DestroyMethodDefImpl md = new DestroyMethodDefImpl("Add");
			IArgDef argDef = new ArgDefImpl("aaa");
			md.AddArgDef(argDef);
			IArgDef argDef2 = new ArgDefImpl("111");
			md.AddArgDef(argDef2);
			cd.AddDestroyMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultDestroyMethodAssembler(cd);
			Hashtable table = new Hashtable();
			assembler.Assemble(table);
			Assert.AreEqual("111",table["aaa"]);
		}

		[Test]
		public void TestAssembleForExpression()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			IDestroyMethodDef md = new DestroyMethodDefImpl();
			md.Expression = "self.Add('aaa','111')";
			cd.AddDestroyMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultDestroyMethodAssembler(cd);
			Hashtable table = new Hashtable();
			assembler.Assemble(table);
			Assert.AreEqual("111",table["aaa"]);
		}

		[Test]
		public void TestAssembleIllegalArgument()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			IDestroyMethodDef md = new DestroyMethodDefImpl("Add");
			cd.AddDestroyMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultDestroyMethodAssembler(cd);
			Hashtable table = new Hashtable();
			try
			{
				assembler.Assemble(table);
				Assert.Fail("1");
			}
			catch(MethodNotFoundRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		public interface IFoo
		{
			string HogeName{ get; }
		}

		public class A : IFoo
		{
			private IHoge hoge_;

			public A()
			{
			}

			public IHoge Hoge
			{
				get { return hoge_; }
				set{ hoge_ = value; }
			}

			public string HogeName
			{
				get { return hoge_.Name; }
			}
		}

		public interface IHoge
		{
			string Name { get; }
		}

		public class B : IHoge
		{
			public string Name
			{
				get { return "B"; }
			}
		}

	}
}
