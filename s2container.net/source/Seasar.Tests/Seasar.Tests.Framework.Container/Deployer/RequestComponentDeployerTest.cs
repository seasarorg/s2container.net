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
using System.Web;
using NUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Container.Impl;

namespace TestSeasar.Framework.Container.Deployer
{
	[TestFixture]
	public class RequestComponentDeployerTest
	{
		private IS2Container container;

		[SetUp]
		public void SetUp()
		{
			HttpRequest request = new HttpRequest("hello.html", "http://localhost/hello.html", "");
			HttpResponse response = new HttpResponse(null);
			HttpContext.Current = new HttpContext(request, response);
			container = new S2ContainerImpl();
			container.HttpContext = HttpContext.Current;
		}

		[Test]
		public void TestDeployAutoAutoConstructor()
		{
			IComponentDef cd = new ComponentDefImpl(typeof(Foo), "foo");
			container.Register(cd);
			IComponentDeployer deployer = new RequestComponentDeployer(cd);
			Foo foo = (Foo) deployer.Deploy(typeof(Foo));
			Assert.AreSame(foo, HttpContext.Current.Items["foo"]);
			Assert.AreSame(foo, deployer.Deploy(typeof(Foo)));
		}

		[Test]
		public void TestDeployAspect1()
		{
			container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl1));

			IAspectDef ad = new AspectDefImpl();
			ad.Expression = "plusOne";
			ad.Container = container;
			cd.AddAspeceDef(ad);
			ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
			container.Register(plusOneCd);
			container.Register(cd);

			IComponentDeployer deployer = new RequestComponentDeployer(cd);
			ICulc culc = (ICulc) deployer.Deploy(typeof(ICulc));
			Assert.AreEqual(1, culc.Count());
		}

		[Test]
		public void TestDeployAspect2()
		{
			container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

			IAspectDef ad = new AspectDefImpl();
			ad.Expression = "plusOne";
			ad.Container = container;
			cd.AddAspeceDef(ad);
			ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
			container.Register(plusOneCd);
			container.Register(cd);

			IComponentDeployer deployer = new RequestComponentDeployer(cd);
			CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
			PlusOneInterceptor.Count = 0;
			Assert.AreEqual(1, culc.Count());
		}

		[Test]
		public void TestDeployAspect3()
		{
			container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(CulcImpl2));

			IAspectDef ad = new AspectDefImpl();
			ad.Expression = "plusOne";
			ad.Container = container;

			IAspectDef ad2 = new AspectDefImpl();
			ad2.Expression = "plusOne";
			ad2.Container = container;

			cd.AddAspeceDef(ad);
			cd.AddAspeceDef(ad2);
			ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
			container.Register(plusOneCd);
			container.Register(cd);

			IComponentDeployer deployer = new RequestComponentDeployer(cd);
			CulcImpl2 culc = (CulcImpl2) deployer.Deploy(typeof(CulcImpl2));
			PlusOneInterceptor.Count = 0;
			Assert.AreEqual(2, culc.Count());
		}

		public class Foo
		{
			private string message_;

			public string Message
			{
				set { message_ = value; }
				get { return message_; }
			}
		}

		public class PlusOneInterceptor : IMethodInterceptor
		{
			public static int Count = 0;
			public object Invoke(IMethodInvocation invocation)
			{
				++Count;
				invocation.Proceed();
				return Count;
			}
		}

		public interface ICulc
		{
			int Count();
		}

		public class CulcImpl1 : ICulc
		{
			public int Count()
			{
				return 0;
			}
		}

		public class CulcImpl2 : MarshalByRefObject, ICulc
		{
			public int Count()
			{
				return 0;
			}
		}
	}
}
