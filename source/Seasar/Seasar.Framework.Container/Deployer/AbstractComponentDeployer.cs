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
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Deployer
{
	/// <summary>
	/// AbstractComponentDeployer ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class AbstractComponentDeployer : IComponentDeployer
	{
		private IComponentDef componentDef_;
		private IConstructorAssembler constructorAssembler_;
		private IPropertyAssembler propertyAssembler_;
		private IMethodAssembler initMethodAssembler_;
		private IMethodAssembler destroyMethodAssembler_;

		public AbstractComponentDeployer(IComponentDef componentDef)
		{
			componentDef_ = componentDef;
			this.SetupAssembler();
		}

		#region ComponentDeployer ÉÅÉìÉo

		public virtual object Deploy(Type receiveType)
		{
			
			return null;
		}

		public virtual void InjectDependency(object outerComponent)
		{
			
		}

		public virtual void Init()
		{
			
		}

		#endregion

		public virtual void Destroy()
		{
			
		}

		protected object GetProxy(Type receiveType)
		{
			if(receiveType == null) return null;
			return ComponentDef.GetProxy(receiveType);
		}

		protected IComponentDef ComponentDef
		{
			get { return componentDef_; }
		}

		protected IConstructorAssembler ConstructorAssembler
		{
			get { return constructorAssembler_; }
		}

		protected IPropertyAssembler PropertyAssembler
		{
			get { return propertyAssembler_; }
		}

		protected IMethodAssembler InitMethodAssembler
		{
			get { return initMethodAssembler_; }
		}

		protected IMethodAssembler DestroyMethodAssembler
		{
			get { return destroyMethodAssembler_; }
		}

		private void SetupAssembler()
		{
			string autoBindingMode = componentDef_.AutoBindingMode;
			if(AutoBindingUtil.IsAuto(autoBindingMode))
			{
				this.SetupAssemblerForAuto();
			}
			else if(AutoBindingUtil.IsConstructor(autoBindingMode))
			{
				this.SetupAssemblerForConstructor();
			}
			else if(AutoBindingUtil.IsProperty(autoBindingMode))
			{
				this.SetupAssemblerForProperty();
			}
			else if(AutoBindingUtil.IsNone(autoBindingMode))
			{
				this.SetupAssemblerForNone();
			}
			else
			{
				throw new ArgumentException(autoBindingMode);
			}
			initMethodAssembler_ = new DefaultInitMethodAssembler(componentDef_);
			destroyMethodAssembler_ = new DefaultDestroyMethodAssembler(componentDef_);
		}

		private void SetupAssemblerForAuto()
		{
			this.SetupConstructorAssemblerForAuto();
			propertyAssembler_ = new AutoPropertyAssembler(componentDef_);
		}

		private void SetupConstructorAssemblerForAuto()
		{
			if(componentDef_.ArgDefSize > 0)
			{
				constructorAssembler_ = new ManualConstructorAssembler(componentDef_);
			}
			else if(componentDef_.Expression != null)
			{
				constructorAssembler_ = 
					new ExpressionConstructorAssembler(componentDef_);
			}
			else
			{
				constructorAssembler_ = new AutoConstructorAssembler(componentDef_);
			}
		}

		private void SetupAssemblerForConstructor()
		{
			this.SetupConstructorAssemblerForAuto();
			propertyAssembler_ = new ManualPropertyAssembler(componentDef_);
		}

		private void SetupAssemblerForProperty()
		{
			if(componentDef_.Expression != null)
			{
				constructorAssembler_ = 
					new ExpressionConstructorAssembler(componentDef_);
			}
			else
			{
				constructorAssembler_ = new ManualConstructorAssembler(componentDef_);
			}
			propertyAssembler_ = new AutoPropertyAssembler(componentDef_);
		}

		private void SetupAssemblerForNone()
		{
			if(componentDef_.ArgDefSize > 0)
			{
				constructorAssembler_ = new ManualConstructorAssembler(componentDef_);
			}
			else if(componentDef_.Expression != null)
			{
				constructorAssembler_ = 
					new ExpressionConstructorAssembler(componentDef_);
			}
			else
			{
				constructorAssembler_ = new DefaultConstructorAssembler(componentDef_);
			}
			if(componentDef_.PropertyDefSize > 0)
			{
				propertyAssembler_ = new ManualPropertyAssembler(componentDef_);
			}
			else
			{
				propertyAssembler_ = new DefaultPropertyAssembler(componentDef_);
			}
		}

	}
}
