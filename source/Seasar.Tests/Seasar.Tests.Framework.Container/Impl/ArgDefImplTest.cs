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
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Impl
{
	/// <summary>
	/// ArgDefImplTest の概要の説明です。
	/// </summary>
	[TestFixture]
	public class ArgDefImplTest
	{
		[Test]
		public void TestSetExpression()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "hoge";
			cd.AddArgDef(ad);
			container.Register(cd);
			ComponentDefImpl cd2 = new ComponentDefImpl(typeof(B), "hoge");
			container.Register(cd2);
			container.Register(typeof(C));
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestSetChildComponentDef()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ComponentDefImpl cd2 = new ComponentDefImpl(typeof(B));
			ad.ChildComponentDef = cd2;
			cd.AddArgDef(ad);
			container.Register(cd);
			container.Register(typeof(C));
			A a = (A) container.GetComponent(typeof(A));
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestPrototype()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(StrHolderImpl), "foo");
			cd.InstanceMode = "prototype";
			ComponentDefImpl cd2 = new ComponentDefImpl(typeof(StrFacadeImpl));
			cd2.InstanceMode = "prototype";
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "foo";
			cd2.AddArgDef(ad);
			container.Register(cd);
			container.Register(cd2);
			IStrFacade facade1 = (IStrFacade) container.GetComponent(typeof(IStrFacade));
			IStrFacade facade2 = (IStrFacade) container.GetComponent(typeof(IStrFacade));
			facade1.Str = "aaa";
			facade2.Str = "bbb";
			Assert.AreEqual("aaa", facade1.Str);
			Assert.AreEqual("bbb", facade2.Str);
		}

		[Test]
		public void TestValueEnumType()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "System.Data.DbType.String";
			cd.AddArgDef(ad);
			container.Register(cd);
			Assert.AreEqual(System.Data.DbType.String, ad.Value);
			ad.Expression = "System.Data.DbType.Decimal";
			Assert.AreEqual(System.Data.DbType.Decimal, ad.Value);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void TestValueEnumTypeInvalidExpression()
		{
			// 列挙定数の名前の解析失敗時、
			// Seasar.Framework.Exceptions.JScriptEvaluateRuntimeException
			// を返すようにしたほうがいいかも
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "System.Data.DbType.Zzz";
			cd.AddArgDef(ad);
			container.Register(cd);
			object value = ad.Value;
		}

		[Test]
		public void TestValueType()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "System.Data.UniqueConstraint";
			cd.AddArgDef(ad);
			container.Register(cd);
			Assert.AreSame(typeof(System.Data.UniqueConstraint), ad.Value);
		}

		[Test]
		[ExpectedException(typeof(Seasar.Framework.Exceptions.JScriptEvaluateRuntimeException))]
		public void TestValueTypeInvalidExpression()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef ad = new ArgDefImpl();
			ad.Expression = "System.Data.Zzz";
			cd.AddArgDef(ad);
			container.Register(cd);
			object value = ad.Value;
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

		public class A2
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
			private A2 a2_;

			public A2 A2
			{
				set { a2_ = value; }
			}

			public string Name
			{
				get { return "C"; }
			}

			public string HogeName
			{
				get { return a2_.HogeName; }
			}
		}

		public interface IStrHolder
		{
			string Str { set; get; }
		}

		public class StrHolderImpl : IStrHolder
		{
			private string str_;

			public string Str
			{
				set { str_ = value; }
				get { return str_; }
			}
		}

		public interface IStrFacade
		{
			string Str { set; get; }
		}

		public class StrFacadeImpl : IStrFacade
		{
			private IStrHolder strHolder_;

			public StrFacadeImpl(IStrHolder strHolder)
			{
				strHolder_ = strHolder;
			}

			public string Str
			{
				set { strHolder_.Str = value; }
				get { return strHolder_.Str; }
			}
		}
	}
}
