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
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using Seasar.Framework.Beans;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Assembler
{
	/// <summary>
	/// DefaultInitMethodAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class DefaultInitMethodAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			IInitMethodDef md = new InitMethodDefImpl("Add");
			IArgDef argDef = new ArgDefImpl("aaa");
			md.AddArgDef(argDef);
			IArgDef argDef2 = new ArgDefImpl("111");
			md.AddArgDef(argDef2);
			cd.AddInitMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			Hashtable table = new Hashtable();
			assembler.Assemble(table);
			Assert.AreEqual("111",table["aaa"]);
		}

		[Test]
		public void TestAssembleForExpression()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			IInitMethodDef md = new InitMethodDefImpl();
			md.Expression = "self.Add('aaa','111')";
			cd.AddInitMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			Hashtable table = new Hashtable();
			assembler.Assemble(table);
			Assert.AreEqual("111",table["aaa"]);
		}

		[Test]
		public void TestAssembleForAuto()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(B));
			IInitMethodDef md = new InitMethodDefImpl("Bbb");
			cd.AddInitMethodDef(md);
			container.Register(cd);
			container.Register(typeof(ArrayList));
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			B b = new B();
			assembler.Assemble(b);
			Assert.AreEqual(0,b.ValueSize);
		}

		[Test]
		public void TestAssembleIllegalArgument()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
			IInitMethodDef md = new InitMethodDefImpl("Add");
			cd.AddInitMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			Hashtable table = new Hashtable();
			try
			{
				assembler.Assemble(table);
				Assert.Fail();
			}
			catch(MethodNotFoundRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestAssembleIllegalArgument2()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(B));
			IInitMethodDef md = new InitMethodDefImpl("SetAAA");
			IArgDef argDef = new ArgDefImpl("aaa");
			md.AddArgDef(argDef);
			cd.AddInitMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			try
			{
				assembler.Assemble(new B());
				Assert.Fail();
			}
			catch(MethodNotFoundRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestAssembleField()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Int32));
			IInitMethodDef md = new InitMethodDefImpl();
			md.Expression = "out.WriteLine(Int32.MinValue)";
			cd.AddInitMethodDef(md);
			container.Register(cd);
			IMethodAssembler assembler = new DefaultInitMethodAssembler(cd);
			assembler.Assemble(1);
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
				set { hoge_ = value; }
			}

			public string HogeName
			{
				get
				{
					return hoge_.Name;
				}
			}
		}

		public interface IHoge
		{
			string Name { get; }
		}

		public class B : IHoge
		{
			private IList values_;

			public string Name
			{
				get
				{
					return "B";
				}
			}
			
			public void SetAaa(int aaa)
			{
			}

			public void Bbb(IList values)
			{
				values_ = values;
			}

			public int ValueSize
			{
				get { return values_.Count; }
			}
		}


	}
}
