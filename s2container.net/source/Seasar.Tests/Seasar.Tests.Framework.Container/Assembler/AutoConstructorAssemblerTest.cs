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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Exceptions;

namespace Seasar.Tests.Framework.Container.Assembler
{
	/// <summary>
	/// AutoConstructorAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class AutoConstructorAssemblerTest
	{
		[Test]
		public void TestAssemble()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			container.Register(cd);
			container.Register(typeof(B));
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			A a = (A) assembler.Assemble();
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestAssembleAspect()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			cd.AddAspeceDef(new AspectDefImpl(new TraceInterceptor()));
			container.Register(cd);
			container.Register(typeof(B));
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			A a = (A) assembler.Assemble();
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestAssembleArgNotFound()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			container.Register(cd);
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			A a = (A) assembler.Assemble();
			Assert.AreEqual(null,a.Hoge);
		}

		[Test]
		public void TestAssembleDefaultConstructor()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(D));
			container.Register(cd);
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			D d = (D) assembler.Assemble();
			Assert.AreEqual("", d.Name);
		}

		[Test]
		public void TestAssembleDefaultConstructor2()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hoge));
			cd.AddAspeceDef(new AspectDefImpl(new HogeInterceptor()));
			container.Register(cd);
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			Hoge hoge = (Hoge) assembler.Assemble();
			Assert.AreEqual("hoge",hoge.Name);
		}

		[Test]
		public void TestAssembleAutoNotInterfaceConstructor()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(C));
			container.Register(cd);
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			try
			{
				assembler.Assemble();
				Assert.Fail();
			}
			catch(NoSuchConstructorRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

		[Test]
		public void TestAccessComponentDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hoge));
			ComponentDefInterceptor interceptor = new ComponentDefInterceptor();
			cd.AddAspeceDef(new AspectDefImpl(interceptor));
			container.Register(cd);
			IConstructorAssembler assembler = new AutoConstructorAssembler(cd);
			Hoge hoge = (Hoge) assembler.Assemble();
			Assert.AreEqual("hoge",hoge.Name);
			Assert.AreSame(cd,interceptor.ComponentDef);
		}

		public interface Foo 
		{
			String HogeName { get; }
		}

		public class A :MarshalByRefObject, Foo 
		{
			private Hoge hoge_;
			public A(Hoge hoge) 
			{
				hoge_ = hoge;
			}

			public Hoge Hoge
			{
				get { return hoge_; }
			}

			public string HogeName
			{
				get { return hoge_.Name; }
			}
		}

		public interface Hoge 
		{

			string Name { get; }
		}

		public class B : Hoge 
		{
			public string Name
			{
				get { return "B"; }
			}
		}

		public class C 
		{

			private string name_;

			public C(string name) 
			{
				name_ = name;
			}

			public String Name
			{
				get { return name_; }
			}
		}

		public class D : MarshalByRefObject
		{
			private string name_;
			public D()
			{
				name_ = "";
			}

			public string Name
			{
				get { return name_; }
			}
		}
	
		public class HogeInterceptor : IMethodInterceptor 
		{
			public object Invoke(IMethodInvocation invocation)
			{
				return "hoge";
			}
		}
	
		public class ComponentDefInterceptor : IMethodInterceptor 
		{
			private IComponentDef componentDef_;
				
			public IComponentDef ComponentDef
			{
				get { return componentDef_; }
			}
				
			public Object Invoke(IMethodInvocation invocation)
			{
				IS2MethodInvocation impl = (IS2MethodInvocation) invocation;
				componentDef_ = (IComponentDef) impl.GetParameter(ContainerConstants.COMPONENT_DEF_NAME);
				return "hoge";
			}
		}
	}
}
