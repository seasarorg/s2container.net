#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Xml;
using Seasar.Framework.Util;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Factory
{
	/// <summary>
	/// ComponentTagHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ComponentTagHandler : TagHandler
	{

		public override void Start(TagHandlerContext context, IAttributes attributes)
		{
			IComponentDef componentDef = null;
			string className = attributes["class"];
			Type componentType = null;
			IS2Container container = (IS2Container) context.Peek(0);
			if(className != null)
			{
				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				componentType = ClassUtil.ForName(className,asms);
				if(componentType == null)
					throw new ClassNotFoundRuntimeException(className);
			}
			string name = attributes["name"];
			componentDef = new ComponentDefImpl(componentType,name);
			string instanceMode = attributes["instance"];
			if(instanceMode != null) componentDef.InstanceMode = instanceMode;
			string autoBindingMode = attributes["autoBinding"];
			if(autoBindingMode != null) componentDef.AutoBindingMode = autoBindingMode;
			context.Push(componentDef);
		}

		public override void End(TagHandlerContext context, string body)
		{
			IComponentDef componentDef = (IComponentDef) context.Pop();
			string expression = null;
			if(body != null)
			{
				expression = body.Trim();
				if(!StringUtil.IsEmpty(expression))
				{
					componentDef.Expression = expression;
				}
				else
				{
					expression = null;
				}
			}
			if(componentDef.ComponentType == null
				&& !InstanceModeUtil.IsOuter(componentDef.InstanceMode)
				&& expression == null)
			{
				throw new TagAttributeNotDefinedRuntimeException("component","class");
			}
			if(context.Peek() is IS2Container)
			{
				IS2Container container = (IS2Container) context.Peek();
				container.Register(componentDef);
			}
			else
			{
				IArgDef argDef = (IArgDef) context.Peek();
				argDef.ChildComponentDef = componentDef;
			}
		}

	}
}