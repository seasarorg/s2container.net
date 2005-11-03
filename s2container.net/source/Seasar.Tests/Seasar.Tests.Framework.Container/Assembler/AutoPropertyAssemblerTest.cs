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
	/// AutoPropertyAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class AutoPropertyAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			container.Register(cd);
			container.Register(typeof(B));
			IPropertyAssembler assembler = new AutoPropertyAssembler(cd);
			A a = new A();
			assembler.Assemble(a);
			Assert.AreEqual("B",a.HogeName);
		}

		[Test]
		public void TestAssemble2()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			cd.AddPropertyDef(new PropertyDefImpl("Message","aaa"));
			container.Register(cd);
			container.Register(typeof(B));
			IPropertyAssembler assembler = new AutoPropertyAssembler(cd);
			A a = new A();
			assembler.Assemble(a);
			Assert.AreEqual("B",a.HogeName);
			Assert.AreEqual("aaa",a.Message);
		}

		[Test]
		public void TestAssembleNotInterface()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(DateTime));
			container.Register(cd);
			IPropertyAssembler assembler = new AutoPropertyAssembler(cd);
			DateTime d = new DateTime();
			assembler.Assemble(d);
		}

		[Test]
		public void TestSkipIllegalProperty()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			container.Register(cd);
			IPropertyAssembler assembler = new AutoPropertyAssembler(cd);
			A a = new A();
			assembler.Assemble(a);
		}

		[Test]
		public void TestSkipWarning()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
			container.Register(cd);
			IPropertyAssembler assembler = new AutoPropertyAssembler(cd);
			A2 a2 = new A2();
			assembler.Assemble(a2);
			Assert.AreEqual("B",a2.HogeName);
		}
		
		public interface IFoo
		{
			string HogeName{ get; }
		}

		public class A : IFoo
		{
			private IHoge hoge_;
			private string message_;
			
			public A()
			{
			}

			public IHoge Hoge
			{
				get { return hoge_; }
				set { hoge_ = value; }
			}

			public string Message
			{
				get { return message_; }
				set { message_ = value; }
			}

			public string HogeName
			{
				get
				{
					return hoge_.Name;
				}
			}
		}

		public class A2 : IFoo
		{
			private IHoge hoge_ = new B();

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
			public string Name
			{
				get
				{
					return "B";
				}
			}
		}

		public class C : IHoge
		{
			public string Name
			{
				get
				{
					return "C";
				}
			}
		}


	}
}
