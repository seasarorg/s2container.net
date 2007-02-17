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
using MbUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Impl
{
	/// <summary>
	/// AspectDefImplTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class AspectDefImplTest
	{

		[Test]
		public void TestSetExpression()
		{
			IS2Container container = new S2ContainerImpl();
			IAspectDef ad = new AspectDefImpl();
			ad.Expression = "traceAdvice";
			ad.Container = container;
			ComponentDefImpl cd = new ComponentDefImpl(typeof(TraceInterceptor), "traceAdvice");
			container.Register(cd);
			Assert.AreEqual(typeof(TraceInterceptor), ad.Aspect.MethodInterceptor.GetType());
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
	}

}
