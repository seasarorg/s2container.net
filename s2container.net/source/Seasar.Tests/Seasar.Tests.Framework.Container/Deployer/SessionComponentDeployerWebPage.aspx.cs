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

namespace Seasar.Tests.Framework.Container.Deployer
{
	public class SessionComponentDeployerWebPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label ResultLabel;
	
		public SessionComponentDeployerWebPage()
		{
		}

		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			IS2Container container = new S2ContainerImpl();
			container.HttpContext = HttpContext.Current;
			IComponentDef cd = new ComponentDefImpl(typeof(Foo), "foo");
			container.Register(cd);
			IComponentDeployer deployer = new SessionComponentDeployer(cd);
			Foo foo = (Foo) deployer.Deploy(typeof(Foo));
			Assert.AreSame(foo, this.Session["foo"]);
			Assert.AreSame(foo, deployer.Deploy(typeof(Foo)));

			ComponentDefImpl cd1 = new ComponentDefImpl(typeof(CulcImpl1), "culcTest1");
			IAspectDef ad1 = new AspectDefImpl();
			ad1.Expression = "plusOne";
			ad1.Container = container;
			cd1.AddAspeceDef(ad1);
			ComponentDefImpl plusOneCd = new ComponentDefImpl(typeof(PlusOneInterceptor), "plusOne");
			container.Register(plusOneCd);
			container.Register(cd1);
			IComponentDeployer deployer1 = new SessionComponentDeployer(cd1);
			ICulc culc1 = (ICulc) deployer1.Deploy(typeof(ICulc));
			Assert.AreEqual(1, culc1.Count());

			ComponentDefImpl cd2 = new ComponentDefImpl(typeof(CulcImpl2), "culcTest2");
			IAspectDef ad2 = new AspectDefImpl();
			ad2.Expression = "plusOne";
			ad2.Container = container;
			cd2.AddAspeceDef(ad2);
			container.Register(cd2);
			IComponentDeployer deployer2 = new SessionComponentDeployer(cd2);
			CulcImpl2 culc2 = (CulcImpl2) deployer2.Deploy(typeof(CulcImpl2));
			PlusOneInterceptor.Count = 0;
			Assert.AreEqual(1, culc2.Count());

			ComponentDefImpl cd3 = new ComponentDefImpl(typeof(CulcImpl2), "culcTest3");
			IAspectDef ad3 = new AspectDefImpl();
			ad3.Expression = "plusOne";
			ad3.Container = container;
			IAspectDef ad4 = new AspectDefImpl();
			ad4.Expression = "plusOne";
			ad4.Container = container;
			cd3.AddAspeceDef(ad3);
			cd3.AddAspeceDef(ad4);
			container.Register(cd3);
			IComponentDeployer deployer3 = new SessionComponentDeployer(cd3);
			CulcImpl2 culc3 = (CulcImpl2) deployer3.Deploy(typeof(CulcImpl2));
			PlusOneInterceptor.Count = 0;
			Assert.AreEqual(2, culc3.Count());
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
