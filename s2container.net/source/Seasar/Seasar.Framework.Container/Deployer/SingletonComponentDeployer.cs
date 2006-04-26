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
	/// SingletonComponentDeployer ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class SingletonComponentDeployer : AbstractComponentDeployer
	{
		private object component_;
		private bool instantiating_ = false;

		public SingletonComponentDeployer(IComponentDef componentDef)
			: base(componentDef)
		{
		}

		public override object Deploy(Type receiveType)
		{
			lock(this)
			{
				if(component_ == null) this.Assemble();

				object proxy = GetProxy(receiveType);
				return proxy == null ? component_ : proxy;
			}
		}

		public override void InjectDependency(object outerComponent)
		{
			throw new NotSupportedException("InjectDependency");
		}

		private void Assemble()
		{
			if(instantiating_) throw new CyclicReferenceRuntimeException(
								   this.ComponentDef.ComponentType);
			instantiating_ = true;
			component_ = this.ConstructorAssembler.Assemble();
			instantiating_ = false;
			this.PropertyAssembler.Assemble(component_);
			this.InitMethodAssembler.Assemble(component_);
			if(this.ComponentDef.AspectDefSize > 0)
			{
				AopProxyUtil.WeaveAspect(ref component_,this.ComponentDef);
			}
		}

		public override void Init()
		{
			this.Deploy(ComponentDef.ComponentType);
		}

		public override void Destroy()
		{
			if(component_ == null) return;
			this.DestroyMethodAssembler.Assemble(component_);
			component_ = null;
		}


	}
}
