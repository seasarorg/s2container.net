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
using System.Text;
using System.Collections;
using NUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Impl
{
	/// <summary>
	/// S2ContainerImplTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class S2ContainerImplTest
	{
		[Test]
		public void TestRegister()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(A));
			container.Register(typeof(B));
			container.Register(typeof(B2));
			try
			{
				A a = (A) container.GetComponent(typeof(A));
				Assert.Fail();
			}
			catch(TooManyRegistrationRuntimeException ex)
			{
				Trace.WriteLine(ex.Message);
				Assert.AreEqual(typeof(IHoge), ex.Key);
				Assert.AreEqual(2, ex.ComponentTypes.Length);
				Assert.AreEqual(typeof(B), ex.ComponentTypes[0]);
				Assert.AreEqual(typeof(B2), ex.ComponentTypes[1]);
			}
		}

		[Test]
		public void TestRegisterForAlreadyRegistration()
		{
			IS2Container container = new S2ContainerImpl();
			IComponentDef cd = new ComponentDefImpl(typeof(B), "B");
			IComponentDef cd2 = new ComponentDefImpl(typeof(B2), "B");
			container.Register(cd);
			container.Register(cd2);
			try
			{
				container.GetComponent("B");
				Assert.Fail();
			}
			catch(TooManyRegistrationRuntimeException ex)
			{
				Trace.WriteLine(ex.Message);
				Assert.AreEqual("B", ex.Key);
				Assert.AreEqual(2, ex.ComponentTypes.Length);
				Assert.AreEqual(typeof(B), ex.ComponentTypes[0]);
				Assert.AreEqual(typeof(B2), ex.ComponentTypes[1]);
			}
		}

		[Test]
		public void TestInclude()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(A));
			IS2Container container2 = new S2ContainerImpl();
			container2.Register(typeof(B));
			container.Include(container2);
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestInclude2()
		{
			IS2Container root = new S2ContainerImpl();
			IS2Container child = new S2ContainerImpl();
			child.Namespace = "aaa";
			child.Register("hoge", "hoge");
			root.Include(child);
			IS2Container child2 = new S2ContainerImpl();
			child2.Namespace = "bbb";
			child2.Register("hoge2", "hoge");
			IS2Container grandchild = new S2ContainerImpl();
			grandchild.Namespace = "ccc";
			grandchild.Register("hoge3", "hoge");
			child2.Include(grandchild);
			root.Include(child2);
			Assert.AreEqual("hoge", child.GetComponent("hoge"));
			Assert.AreEqual("hoge3", grandchild.GetComponent("hoge"));
			Assert.AreEqual(child, root.GetComponent("aaa"));
			Assert.AreEqual(child2, root.GetComponent("bbb"));
			Assert.AreEqual("hoge", root.GetComponent("aaa.hoge"));
			Assert.AreEqual("hoge2", root.GetComponent("bbb.hoge"));
			Assert.AreEqual("hoge3", root.GetComponent("ccc.hoge"));
			Assert.AreEqual("hoge", child.GetComponent("aaa.hoge"));
			Assert.AreEqual(false, child.HasComponentDef("bbb.hoge"));
			Assert.AreEqual(false, child.HasComponentDef("ccc.hoge"));
			Assert.AreEqual("hoge2", child2.GetComponent("hoge"));
			Assert.AreEqual("hoge3", child2.GetComponent("ccc.hoge"));
			Assert.AreEqual(0, root.ComponentDefSize);
		}

		[Test]
		public void TestInclude3()
		{
			IS2Container container = new S2ContainerImpl();
			IS2Container child = new S2ContainerImpl();
			child.Path = "aaa.xml";
			IS2Container grandchild = new S2ContainerImpl();
			grandchild.Path = "bbb.xml";
			grandchild.Namespace = "bbb";
			child.Include(grandchild);
			container.Include(child);
			container.Include(grandchild);
			Assert.IsNotNull(container.GetComponent("bbb"));
		}

		[Test]
		public void TestInclude4()
		{
			IS2Container aaa = new S2ContainerImpl();
			aaa.Path = "aaa.xml";
			aaa.Namespace = "aaa";
			IS2Container bbb = new S2ContainerImpl();
			bbb.Path = "bbb.xml";
			IS2Container aaa2 = new S2ContainerImpl();
			aaa2.Path = "aaa.xml";
			aaa2.Namespace = "aaa";
			bbb.Include(aaa2);
			aaa.Include(bbb);
			Assert.IsNotNull(aaa.GetComponentDef("aaa"));
		}

		[Test]
		public void TestInclude5()
		{
			IS2Container container = new S2ContainerImpl();
			IS2Container child = new S2ContainerImpl();
			child.Namespace = "aaa";
			IS2Container child2 = new S2ContainerImpl();
			child2.Namespace = "aaa";
			container.Include(child);
			container.Include(child2);
			try
			{
				container.GetComponent("aaa");
				Assert.Fail();
			}
			catch(TooManyRegistrationRuntimeException ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}

		[Test]
		public void TestInitAndDestroy()
		{
			IS2Container container = new S2ContainerImpl();
			IS2Container child = new S2ContainerImpl();
			IList initList = new ArrayList();
			IList destroyList = new ArrayList();
			IComponentDef componentDef = new ComponentDefImpl(typeof(C), "c1");
			componentDef.AddInitMethodDef(new InitMethodDefImpl("Init"));
			componentDef.AddDestroyMethodDef(new DestroyMethodDefImpl("Destroy"));
			componentDef.AddArgDef(new ArgDefImpl("c1"));
			componentDef.AddArgDef(new ArgDefImpl(initList));
			componentDef.AddArgDef(new ArgDefImpl(destroyList));
			container.Register(componentDef);

			componentDef = new ComponentDefImpl(typeof(C), "c2");
			componentDef.AddInitMethodDef(new InitMethodDefImpl("Init"));
			componentDef.AddDestroyMethodDef(new DestroyMethodDefImpl("Destroy"));
			componentDef.AddArgDef(new ArgDefImpl("c2"));
			componentDef.AddArgDef(new ArgDefImpl(initList));
			componentDef.AddArgDef(new ArgDefImpl(destroyList));
			container.Register(componentDef);

			componentDef = new ComponentDefImpl(typeof(C), "c3");
			componentDef.AddInitMethodDef(new InitMethodDefImpl("Init"));
			componentDef.AddDestroyMethodDef(new DestroyMethodDefImpl("Destroy"));
			componentDef.AddArgDef(new ArgDefImpl("c3"));
			componentDef.AddArgDef(new ArgDefImpl(initList));
			componentDef.AddArgDef(new ArgDefImpl(destroyList));
			child.Register(componentDef);
			container.Include(child);
			
			container.Init();
			Assert.AreEqual(3, initList.Count);
			Assert.AreEqual("c3", initList[0]);
			Assert.AreEqual("c1", initList[1]);
			Assert.AreEqual("c2", initList[2]);
			container.Destroy();
			Assert.AreEqual(3, destroyList.Count);
			Assert.AreEqual("c2", destroyList[0]);
			Assert.AreEqual("c1", destroyList[1]);
			Assert.AreEqual("c3", destroyList[2]);
		}

		[Test]
		public void TestInjectDependency()
		{
			IS2Container container = new S2ContainerImpl();
			IComponentDef cd = new ComponentDefImpl(typeof(Hashtable), "hoge");
			cd.InstanceMode = "outer";
			IInitMethodDef md = new InitMethodDefImpl("Add");
			md.AddArgDef(new ArgDefImpl("aaa"));
			md.AddArgDef(new ArgDefImpl("111"));
			cd.AddInitMethodDef(md);
			container.Register(cd);

			Hashtable table = new Hashtable();
			container.InjectDependency(table);
			Assert.AreEqual("111", table["aaa"]);

			Hashtable table2 = new Hashtable();
			container.InjectDependency(table2, typeof(Hashtable));
			Assert.AreEqual("111", table2["aaa"]);

			Hashtable table3 = new Hashtable();
			container.InjectDependency(table3, "hoge");
			Assert.AreEqual("111", table3["aaa"]);
		}

		[Test]
		public void TestSelf()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(D));
			D d = (D) container.GetComponent(typeof(D));
			Assert.AreSame(container, d.Container);
		}

		[Test]
		public void TestSelf2()
		{
			IS2Container container = new S2ContainerImpl();
			IComponentDef cd = new ComponentDefImpl(typeof(D));
			IPropertyDef pd = new PropertyDefImpl("container");
			pd.Expression = ContainerConstants.CONTAINER_NAME;
			cd.AddPropertyDef(pd);
			container.Register(cd);
			D d = (D) container.GetComponent(typeof(D));
			Assert.AreSame(container, d.Container);
		}

		[Test]
		public void TestConstructor()
		{
			IS2Container container = new S2ContainerImpl();
			Assert.AreEqual(0, container.ComponentDefSize);
		}

		[Test]
		public void TestNamespace()
		{
			IS2Container container = new S2ContainerImpl();
			container.Namespace = "aaa";
			container.Register(typeof(StringBuilder), "bbb");
			Assert.IsNotNull(container.GetComponent("bbb"));
			Assert.IsNotNull(container.GetComponent("aaa.bbb"));
		}

		[Test]
		public void TestGetComponentDef()
		{
			IS2Container aaa = new S2ContainerImpl();
			aaa.Namespace = "aaa";
			IS2Container bbb = new S2ContainerImpl();
			bbb.Namespace = "bbb";
			bbb.Register(typeof(StringBuilder), "hoge");
			aaa.Include(bbb);
			Assert.IsNotNull(aaa.GetComponentDef("bbb.hoge"));
			Assert.IsNotNull(bbb.GetComponentDef("bbb.hoge"));
		}

		[Test]
		public void TestGetComponentDef2()
		{
			IS2Container container = new S2ContainerImpl();
			container.Register(typeof(FooImpl));
			IHoge hoge = (IHoge) container.GetComponent(typeof(IHoge));
			Assert.AreEqual("Foo", hoge.Name);
		}

		public class A
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

		public interface IHoge
		{
			string Name { get; }
		}

		public interface IFoo : IHoge
		{
		}

		public class B : IHoge
		{
			public string Name
			{
				get { return "B"; }
			}
		}

		public class B2 : IHoge
		{
			public string Name
			{
				get { return "B2"; }
			}
		}

		public class C
		{
			private string name_;
			private IList initList_;
			private IList destroyList_;

			public C(string name, IList initList, IList destroyList)
			{
				name_ = name;
				initList_ = initList;
				destroyList_ = destroyList;
			}

			public void Init()
			{
				initList_.Add(name_);
			}

			public void Destroy()
			{
				destroyList_.Add(name_);
			}
		}

		public class D
		{
			private IS2Container container_;

			public IS2Container Container
			{
				get { return container_; }
				set { container_ = value; }
			}
		}

		public class FooImpl : IFoo
		{
			public string Name
			{
				get { return "Foo"; }
			}
		}

	}
}
