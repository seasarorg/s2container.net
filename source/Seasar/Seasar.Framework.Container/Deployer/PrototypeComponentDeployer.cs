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
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Deployer
{
	/// <summary>
	/// PrototypeComponentDeployer ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class PrototypeComponentDeployer : AbstractComponentDeployer
	{
		public PrototypeComponentDeployer(IComponentDef componentDef)
			: base(componentDef)
		{
		}

		public override object Deploy(Type receiveType)
		{
			object component = this.ConstructorAssembler.Assemble();
			this.PropertyAssembler.Assemble(component);
			this.InitMethodAssembler.Assemble(component);
			if(this.ComponentDef.AspectDefSize > 0)
			{
				AopProxyUtil.WeaveAspect(ref component,this.ComponentDef);
			}

			object proxy = GetProxy(receiveType);
			return proxy == null ? component : proxy;
		}

		public override void InjectDependency(object outerComponent)
		{
			throw new NotSupportedException("InjectDependency");
		}


	}
}
