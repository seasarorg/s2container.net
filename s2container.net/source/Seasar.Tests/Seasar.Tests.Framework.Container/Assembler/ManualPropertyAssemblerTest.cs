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
using System.Diagnostics;
using System.Runtime.Remoting;
using NUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Beans;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Exceptions;

namespace Seasar.Tests.Framework.Container.Assembler
{
	/// <summary>
	/// ManualPropertyAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ManualPropertyAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IPropertyDef pd = new PropertyDefImpl("Hoge",new B());
			cd.AddPropertyDef(pd);
			container.Register(cd);
			IPropertyAssembler assembler = new ManualPropertyAssembler(cd);
			A a = new A();
			assembler.Assemble(a);
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestAssembleIllegalProperty()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IPropertyDef pd = new PropertyDefImpl("Hoge");
			pd.Expression = "B";
			cd.AddPropertyDef(pd);
			container.Register(cd);
			IPropertyAssembler assembler = new ManualPropertyAssembler(cd);
			A a = new A();
			try
			{
				assembler.Assemble(a);
				Assert.Fail();
			}
			catch(JScriptEvaluateRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestAssembleIllegalProperty2()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IPropertyDef pd = new PropertyDefImpl("abc", "111");
			cd.AddPropertyDef(pd);
			container.Register(cd);
			IPropertyAssembler assembler = new ManualPropertyAssembler(cd);
			A a = new A();
			try
			{
				assembler.Assemble(a);
				Assert.Fail();
			}
			catch(PropertyNotFoundRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestAssembleIllegalProperty3()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(B));
			IPropertyDef pd = new PropertyDefImpl("Aaa", "abc");
			cd.AddPropertyDef(pd);
			container.Register(cd);
			IPropertyAssembler assembler = new ManualPropertyAssembler(cd);
			B b = new B();
			try
			{
				assembler.Assemble(b);
				Assert.Fail();
			}
			catch(IllegalPropertyRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestWithConstructor()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(B));
			IPropertyDef pd = new PropertyDefImpl("Aaa", 123);
			cd.AddPropertyDef(pd);
			IArgDef ad = new ArgDefImpl("BBB");
			cd.AddArgDef(ad);
			container.Register(cd);
			B b = (B) container.GetComponent(typeof(B));
			Assert.AreEqual("BBB", b.Bbb);
			Assert.AreEqual(123, b.Aaa);
		}

        [Test]
        public void TestAssembleWithAspect()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            ComponentDefImpl cdB = new ComponentDefImpl(typeof(B), "B");
            IPropertyDef pd = new PropertyDefImpl("Hoge");
            pd.Expression = "B";
            cd.AddPropertyDef(pd);
            IAspectDef ad = new AspectDefImpl(new TraceInterceptor());
            cdB.AddAspeceDef(ad);
            container.Register(cd);
            container.Register(cdB);
            IPropertyAssembler assembler = new ManualPropertyAssembler(cd);
            A a = new A();
            assembler.Assemble(a);
            Assert.AreEqual("B", a.HogeName, "1");
            Assert.IsTrue(RemotingServices.IsTransparentProxy(a.Hoge), "2");
        }



		public interface IFoo
		{
			string HogeName { get; }
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
				get { return hoge_.Name; }
			}
		}

		public interface IHoge
		{
			string Name { get; }
		}

		public class B : IHoge
		{
			private int aaa_;
			private string bbb_;

			public B()
			{
			}

			public B(string bbb)
			{
				bbb_ = bbb;
			}
			public string Name
			{
				get { return "B"; }
			}

			public int Aaa
			{
				set { aaa_ = value; }
				get { return aaa_; }
			}

			public string Bbb
			{
				get { return bbb_; }
			}
		}
	}
}
