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
using System.Collections;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Beans;

namespace Seasar.Framework.Container.Impl
{
	/// <summary>
	/// ComponentDefImpl の概要の説明です。
	/// </summary>
	public class ComponentDefImpl : IComponentDef
	{
		private Type componentType_;
		private string componentName_;
		private IS2Container container_;
		private string expression_;
		private ArgDefSupport argDefSupport_ = new ArgDefSupport();
		private PropertyDefSupport propertyDefSupport_ = new PropertyDefSupport();
		private InitMethodDefSupport initMethodDefSupport_ = new InitMethodDefSupport();
		private DestroyMethodDefSupport destroyMethodDefSupport_ = new DestroyMethodDefSupport();
		private AspectDefSupport aspectDefSupport_ = new AspectDefSupport();
		private MetaDefSupport metaDefSupport_ = new MetaDefSupport();
		private string instanceMode_ = ContainerConstants.INSTANCE_SINGLETON;
		private string autoBindingMode_ = ContainerConstants.AUTO_BINDING_AUTO;
		private IComponentDeployer componentDeployer_;
		private IDictionary proxies_ = new Hashtable();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ComponentDefImpl()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="componentType">コンポーネントのType</param>
		public ComponentDefImpl(Type componentType)
			: this(componentType,null)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="componentType">コンポーネントのType</param>
		/// <param name="componentName">コンポーネントの名前</param>
		public ComponentDefImpl(Type componentType,string componentName)
		{
			componentType_ = componentType;
			componentName_ = componentName;
		}

		#region ComponentDef メンバ

		public object GetComponent()
		{
			return this.ComponentDeployer.Deploy(ComponentType);
		}

		public object GetComponent(Type receiveType)
		{
			return this.ComponentDeployer.Deploy(receiveType);
		}

		public void InjectDependency(Object outerComponent)
		{
			
			this.ComponentDeployer.InjectDependency(outerComponent);
		}

		public IS2Container Container
		{
			get
			{
				
				return container_;
			}
			set
			{
				
				container_ = value;
				argDefSupport_.Container = value;
				metaDefSupport_.Container = value;
				metaDefSupport_.Container = value;
				propertyDefSupport_.Container = value;
				initMethodDefSupport_.Container = value;
				destroyMethodDefSupport_.Container = value;
				aspectDefSupport_.Container = value;
			}
		}

		public Type ComponentType
		{
			get
			{
				
				return componentType_;
			}
		}

		public string ComponentName
		{
			get
			{
				
				return componentName_;
			}
		}

		public string AutoBindingMode
		{
			get
			{
				
				return autoBindingMode_;
			}
			set
			{
				
				if(AutoBindingUtil.IsAuto(value)
					|| AutoBindingUtil.IsConstructor(value)
					|| AutoBindingUtil.IsProperty(value)
					|| AutoBindingUtil.IsNone(value))
				{
					autoBindingMode_ = value;
				} 
				else 
				{
					throw new ArgumentException(value);
				}
			}
		}

		public string InstanceMode
		{
			get
			{
				
				return instanceMode_;
			}
			set
			{
				
				if(InstanceModeUtil.IsSingleton(value)
					|| InstanceModeUtil.IsPrototype(value)
					|| InstanceModeUtil.IsRequest(value)
					|| InstanceModeUtil.IsSession(value)
					|| InstanceModeUtil.IsOuter(value))
				{
					instanceMode_ = value;
				}
				else
				{
					throw new ArgumentException(value);
				}
			}
		}

		public string Expression
		{
			get
			{
				
				return expression_;
			}
			set
			{
				
				expression_ = value;
			}
		}

		public void Init()
		{
			
			this.ComponentDeployer.Init();
		}

		public object GetProxy(Type proxyType)
		{
			if(proxyType == null) return null;
			return proxies_[proxyType];
		}

		public void AddProxy(Type proxyType, object proxy)
		{
			proxies_[proxyType] = proxy;
		}

		public void Destroy()
		{
			this.ComponentDeployer.Destroy();
		}

		#endregion

		#region IArgDefAware メンバ

		public void AddArgDef(IArgDef argDef)
		{
			
			argDefSupport_.AddArgDef(argDef);
		}

		public int ArgDefSize
		{
			get
			{
				
				return argDefSupport_.ArgDefSize;
			}
		}

		public IArgDef GetArgDef(int index)
		{
			
			return argDefSupport_.GetArgDef(index);
		}

		#endregion

		#region PropertyDefAware メンバ

		public void AddPropertyDef(IPropertyDef propertyDef)
		{
			
			propertyDefSupport_.AddPropertyDef(propertyDef);
		}

		public int PropertyDefSize
		{
			get
			{
				
				return propertyDefSupport_.PropertyDefSize;
			}
		}

		public IPropertyDef GetPropertyDef(int index)
		{
			
			return propertyDefSupport_.GetPropertyDef(index);
		}

		public IPropertyDef GetPropertyDef(string propertyName)
		{
			
			if(this.HasPropertyDef(propertyName))
			{
				return propertyDefSupport_.GetPropertyDef(propertyName);
			}
			else
			{
				throw new PropertyNotFoundRuntimeException(componentType_,propertyName);
			}
		}

		public bool HasPropertyDef(string propertyName)
		{
			
			return propertyDefSupport_.HasPropertyDef(propertyName);
		}

		#endregion

		#region InitMethodDefAware メンバ

		public void AddInitMethodDef(IInitMethodDef methodDef)
		{
			
			initMethodDefSupport_.AddInitMethodDef(methodDef);
		}

		public int InitMethodDefSize
		{
			get
			{
				
				return initMethodDefSupport_.InitMethodDefSize;
			}
		}

		public IInitMethodDef GetInitMethodDef(int index)
		{
			
			return initMethodDefSupport_.GetInitMethodDef(index);
		}

		#endregion

		#region DestroyMethodDefAware メンバ

		public void AddDestroyMethodDef(IDestroyMethodDef methodDef)
		{
			
			destroyMethodDefSupport_.AddDestroyMethodDef(methodDef);
		}

		public int DestroyMethodDefSize
		{
			get
			{
				
				return destroyMethodDefSupport_.DestroyMethodDefSize;
			}
		}

		public IDestroyMethodDef GetDestroyMethodDef(int index)
		{
			
			return destroyMethodDefSupport_.GetDestroyMethodDef(index);
		}

		#endregion

		#region AspectDefAware メンバ

		public void AddAspeceDef(IAspectDef aspectDef)
		{
			
			aspectDefSupport_.AddAspectDef(aspectDef);
		}

		public int AspectDefSize
		{
			get
			{
				
				return aspectDefSupport_.AspectDefSize;
			}
		}

		public IAspectDef GetAspectDef(int index)
		{
			
			return aspectDefSupport_.GetAspectDef(index);
		}

		#endregion

		#region MetaDefAware メンバ

		public void AddMetaDef(IMetaDef metaDef)
		{
			
			metaDefSupport_.AddMetaDef(metaDef);
		}

		public int MetaDefSize
		{
			get
			{
				
				return metaDefSupport_.MetaDefSize;
			}
		}

		public IMetaDef GetMetaDef(int index)
		{
			
			return metaDefSupport_.GetMetaDef(index);
		}

		public IMetaDef GetMetaDef(string name)
		{
			
			return metaDefSupport_.GetMetaDef(name);
		}

		public IMetaDef[] GetMetaDefs(string name)
		{
			
			return metaDefSupport_.GetMetaDefs(name);
		}

		#endregion

		/// <summary>
		/// ComponentDeployer
		/// </summary>
		private IComponentDeployer ComponentDeployer
		{
			get
			{
				lock(this)
				{
					if(componentDeployer_ == null)
					{
						componentDeployer_ = ComponentDeployerFactory.Create(this);
					}
					return componentDeployer_;
				}
			}
		}
	}
}
