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
using System.Reflection;
using System.Runtime.Remoting;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Log;
using Seasar.Framework.Util;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Assembler
{
	/// <summary>
	/// AutoPropertyAssembler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class AutoPropertyAssembler : AbstractPropertyAssembler
	{
		private static Logger logger_ = Logger.GetLogger(typeof(AutoPropertyAssembler));
		
		public AutoPropertyAssembler(IComponentDef componentDef)
			: base(componentDef)
		{
		}

		public override void Assemble(object component)
		{
			Type type = component.GetType();
			if ( RemotingServices.IsTransparentProxy(component) )
			{
				AopProxy aopProxy = RemotingServices.GetRealProxy(component) as AopProxy;
                if (aopProxy != null)
                {
                    type = 	aopProxy.TargetType;
                }
			}

			IS2Container container = this.ComponentDef.Container;
			foreach(PropertyInfo property in type.GetProperties())
			{
				object value = null;
				string propName = property.Name;
				if(this.ComponentDef.HasPropertyDef(propName))
				{
					IPropertyDef propDef = this.ComponentDef.GetPropertyDef(propName);
					value = this.GetValue(propDef,component);
					this.SetValue(property,component,value);
				}
				else if(property.CanWrite 
					&& AutoBindingUtil.IsSuitable(property.PropertyType))
				{
					try
					{
						value = container.GetComponent(property.PropertyType);
					}
					catch(ComponentNotFoundRuntimeException cause)
					{
						if(property.CanRead 
							&& property.GetValue(component,null) != null)
						{
							continue;
						}
						logger_.Log("WSSR0008",
							new object[] {this.GetComponentType(
											 component).FullName, propName},cause);
						continue;
					}
					this.SetValue(property,component,value);
				}

			}
		}

	}
}
