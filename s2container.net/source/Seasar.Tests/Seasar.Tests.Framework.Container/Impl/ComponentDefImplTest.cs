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
using System.IO;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Container.Impl
{
	/// <summary>
	/// ComponentDefImplTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ComponentDefImplTest
	{
		[SetUp]
		public void SetUp()
		{
			FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
				Assembly.GetExecutingAssembly()) + ".config");
			XmlConfigurator.Configure(LogManager.GetRepository(), info);
		}

		[Test]
		public void TestGetComponentForType3()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			container.Register(cd);
			container.Register(typeof(B));
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("B", a.HogeName);
			Assert.AreSame(a, container.GetComponent(typeof(A)));
		}

		[Test]
		public void TestGetComponentForType2()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
			container.Register(cd);
			container.Register(typeof(B));
			A2 a2 = (A2) container.GetComponent(typeof(A2));
			Assert.AreEqual("B", a2.HogeName);
		}

		[Test]
		public void TestGetComponentForArgDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Decimal), "num");
			cd.AddArgDef(new ArgDefImpl(123));
			container.Register(cd);
			Assert.AreEqual(new Decimal(123), container.GetComponent("num"));
		}

		[Test]
		public void TestGetComponentForPropertyDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A2));
			cd.AddPropertyDef(new PropertyDefImpl("hoge", new B()));
			container.Register(cd);
			A2 a2 = (A2) container.GetComponent(typeof(A2));
			Assert.AreEqual("B", a2.HogeName);
		}

		[Test]
		public void TestGetComponentForMethodDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable), "myTable");
			IInitMethodDef md = new InitMethodDefImpl("Add");
			md.AddArgDef(new ArgDefImpl("aaa"));
			md.AddArgDef(new ArgDefImpl("hoge"));
			cd.AddInitMethodDef(md);
			container.Register(cd);
			Hashtable table = (Hashtable) container.GetComponent("myTable");
			Assert.AreEqual("hoge", table["aaa"]);
		}

		[Test]
		public void TestGetComponentForAspectDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			cd.AddAspeceDef(new AspectDefImpl(new TraceInterceptor()));
			container.Register(cd);
			container.Register(typeof(B));
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestGetComponentForExpression()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(Object), "obj");
			ComponentDefImpl cd = new ComponentDefImpl(null, "hash");
			cd.Expression = "container.GetComponent('obj').GetHashCode()";
			container.Register(cd);
			Assert.IsNotNull(container.GetComponent("hash"));
		}

		[Test]
		public void TestCyclicReference()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(A2));
			container.Register(typeof(C));
			A2 a2 = (A2) container.GetComponent(typeof(A2));
			C c = (C) container.GetComponent(typeof(C));
			Assert.AreEqual("C", a2.HogeName);
			Assert.AreEqual("C", c.HogeName);
		}

		[Test]
		public void TestInit()
		{
			IComponentDef cd = new ComponentDefImpl(typeof(D));
			cd.AddInitMethodDef(new InitMethodDefImpl("Init"));
			cd.Init();
			D d = (D) cd.GetComponent();
			Assert.AreEqual(true, d.IsInited);
		}

		[Test]
		public void TestDestroy()
		{
			IComponentDef cd = new ComponentDefImpl(typeof(D));
			cd.AddDestroyMethodDef(new DestroyMethodDefImpl("Destroy"));
			D d = (D) cd.GetComponent();
			cd.Destroy();
			Assert.AreEqual(true, d.IsDestroyed);
		}

		public interface IFoo
		{
			string HogeName { get; }
		}

		public class A : MarshalByRefObject
		{
			private IHoge hoge_;

			public A(IHoge hoge)
			{
				hoge_ = hoge;
			}

			public string HogeName
			{
				get { return hoge_.Name; }
			}
		}

		public class A2 : IFoo
		{
			private IHoge hoge_;

			public IHoge Hoge
			{
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
			public string Name
			{
				get { return "B"; }
			}
		}

		public class C : IHoge
		{
			private IFoo foo_;

			public IFoo Foo
			{
				set { foo_ = value; }
			}

			public string Name
			{
				get { return "C"; }
			}

			public string HogeName
			{
				get { return foo_.HogeName; }
			}
		}

		public class D
		{
			private bool inited_ = false;
			private bool destroyed_ = false;

			public bool IsInited
			{
				get { return inited_; }
			}

			public bool IsDestroyed
			{
				get { return destroyed_; }
			}

			public void Init()
			{
				inited_ = true;
			}

			public void Destroy()
			{
				destroyed_ = true;
			}
		}	
	}
}
