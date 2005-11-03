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
using Seasar.Framework.Container.Factory;

namespace Seasar.Framework.Container.Web
{
	/// <summary>
	/// S2HttpModule ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class S2HttpModule : IHttpModule
	{

		public S2HttpModule()
		{
		}

		#region IHttpModule ÉÅÉìÉo

		public void Init(HttpApplication context)
		{
			context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
			context.ReleaseRequestState += new EventHandler(context_ReleaseRequestState);
			
			SingletonS2ContainerFactory.Init();
			
		}

		public void Dispose()
		{
			SingletonS2ContainerFactory.Destroy();
		}

		#endregion

		private void context_AcquireRequestState(object sender, EventArgs e)
		{
			HttpApplication ha = (HttpApplication) sender;
			IHttpHandler handler = ha.Context.Handler;
			
			IS2Container container = SingletonS2ContainerFactory.Container;
			container.HttpContext = HttpContext.Current;
			string componentName = ha.Request.Path;
			if(container.HasComponentDef(componentName))
			{
				container.InjectDependency(handler, componentName);
			}
		}

		private void context_ReleaseRequestState(object sender, EventArgs e)
		{
			IS2Container container = SingletonS2ContainerFactory.Container;
			container.HttpContext = null;
		}
	}
}
