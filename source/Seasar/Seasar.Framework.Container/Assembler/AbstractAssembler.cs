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
using Seasar.Framework.Log;

namespace Seasar.Framework.Container.Assembler
{
	/// <summary>
	/// AbstractAssembler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public abstract class AbstractAssembler
	{
		private static Logger logger_ = Logger.GetLogger(typeof(AbstractAssembler));
		private IComponentDef componentDef_;

		public AbstractAssembler(IComponentDef componentDef)
		{
			componentDef_ = componentDef;		
		}

		protected IComponentDef ComponentDef
		{
			get { return componentDef_; }
		}

		protected Type GetComponentType()
		{
			return componentDef_.ComponentType;
		}

		protected Type GetComponentType(object component)
		{
			Type type = componentDef_.ComponentType;
			if(type != null)
			{
				return type;
			} 
			else 
			{
				return component.GetType();
			}
		}

		protected object[] GetArgs(Type[] argTypes)
		{
			object[] args = new Object[argTypes.Length];
			for(int i = 0; i < argTypes.Length; i++)
			{
				try 
				{
					args[i] = this.ComponentDef.Container.GetComponent(argTypes[i]);
				} 
				catch (ComponentNotFoundRuntimeException ex) 
				{
					logger_.Log("WSSR0007",
						new object[] {
										 this.ComponentDef.ComponentType.FullName,
										 ex.Message});
					args[i] = null;
				}
			}
			return args;
		}
	}
}
