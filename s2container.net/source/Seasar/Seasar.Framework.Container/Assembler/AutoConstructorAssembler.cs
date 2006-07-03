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
using Seasar.Framework.Util;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Assembler
{
	/// <summary>
	/// AutoConstructorAssembler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class AutoConstructorAssembler : AbstractConstructorAssembler
	{
		public AutoConstructorAssembler(IComponentDef componentDef)
			: base(componentDef)
		{
		}

		public override object Assemble()
		{
			ConstructorInfo constructor =this.GetSuitableConstructor();
			object obj;
			if(constructor == null)
			{
				if(this.ComponentDef.ComponentType.IsInterface)
				{
					obj = new object();
				}
				else
				{
					return this.AssembleDefault();
				}
			}
			else
			{
				object[] args = this.GetArgs(
					ParameterUtil.GetParameterTypes(constructor.GetParameters()));
				obj = ConstructorUtil.NewInstance(constructor,args);
			}
			if(this.ComponentDef.AspectDefSize > 0)
			{
				AopProxyUtil.AspectWeaver(ref obj,this.ComponentDef);
			}
			return obj;
		}

		private ConstructorInfo GetSuitableConstructor()
		{
			int argSize = -1;
			ConstructorInfo constructor = null;
			ConstructorInfo[] constructors =
				this.ComponentDef.ComponentType.GetConstructors();
			for(int i = 0; i < constructors.Length; ++i)
			{
				int tempArgSize = constructors[i].GetParameters().Length;
				if(tempArgSize == 0) return null;
				if(tempArgSize > argSize
					&& AutoBindingUtil.IsSuitable(constructors[i].GetParameters()))
				{
					constructor = constructors[i];
					argSize = tempArgSize;
				}
			}
			return constructor;
		}		
	}
}
