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
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;

namespace Seasar.Framework.Unit
{
	/// <summary>
	/// S2FrameworkTestCase ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class S2FrameworkTestCaseBase
	{
		private IS2Container container;

		public S2FrameworkTestCaseBase()
		{
		}

		public IS2Container Container
		{
			get { return container; }
			set { container = value; }
		}

		public object GetComponent(string componentName)
		{
			return container.GetComponent(componentName);
		}

		public object GetComponent(Type componentClass)
		{
			return container.GetComponent(componentClass);
		}

		public IComponentDef GetComponentDef(string componentName)
		{
			return container.GetComponentDef(componentName);
		}

		public IComponentDef GetComponentDef(Type componentClass)
		{
			return container.GetComponentDef(componentClass);
		}

		public void Register(Type componentClass)
		{
			container.Register(componentClass);
		}

		public void Register(Type componentClass, string componentName)
		{
			container.Register(componentClass, componentName);
		}

		public void Register(object component)
		{
			container.Register(component);
		}

		public void Register(object component, string componentName)
		{
			container.Register(component, componentName);
		}

		public void Register(IComponentDef componentDef)
		{
			container.Register(componentDef);
		}

		public void Include(string path)
		{
			S2ContainerFactory.Include(Container, convertPath(path));
		}

		protected String convertPath(String path)
		{
			if (ResourceUtil.GetResourceNoException(path, this.GetType().Assembly) != null)
			{
				return path;
			}
			if (path.IndexOf('/') > 0)
			{
				return path;
			}				
			string prefix = this.GetType().FullName.Replace('.', '/');
			int pos = (prefix.LastIndexOf("/") + 1);
			prefix = prefix.Substring(0, pos);
			return prefix + path;
		}
	}
}
