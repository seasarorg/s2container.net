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
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Framework.Container.Util
{
	/// <summary>
	/// AopProxyUtil ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public sealed class AopProxyUtil
	{
		private AopProxyUtil()
		{
		}

		public static void AspectWeaver(ref object target,IComponentDef componentDef)
		{
			if(componentDef.AspectDefSize == 0) return;
			Hashtable parameters = new Hashtable();
			parameters[ContainerConstants.COMPONENT_DEF_NAME] = componentDef;
            
			Type[] interfaces = componentDef.ComponentType.GetInterfaces();
			if(componentDef.ComponentType.IsMarshalByRef)
			{
				AopProxy aopProxy = new AopProxy(componentDef.ComponentType,
					GetAspects(componentDef),parameters,target);
				componentDef.AddProxy(componentDef.ComponentType, aopProxy.Create());
			}
			else if(componentDef.ComponentType.IsInterface)
			{
				AopProxy aopProxy = new AopProxy(componentDef.ComponentType,
					GetAspects(componentDef),parameters,target);
				target = aopProxy.Create();
			}
			foreach(Type interfaceType in interfaces)
			{
				AopProxy aopProxy = new AopProxy(interfaceType,
					GetAspects(componentDef), parameters, target);
				componentDef.AddProxy(interfaceType, aopProxy.Create());
			}
		}

		private static IAspect[] GetAspects(IComponentDef componentDef)
		{
			int size = componentDef.AspectDefSize;
			IAspect[] aspects = new IAspect[size];
			for(int i = 0; i < size; ++i)
			{
				aspects[i] = componentDef.GetAspectDef(i).Aspect;
			}
			return aspects;
		}
	}
}
