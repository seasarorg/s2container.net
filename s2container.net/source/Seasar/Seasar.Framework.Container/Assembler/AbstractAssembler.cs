#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Log;

namespace Seasar.Framework.Container.Assembler
{
	/// <summary>
	/// AbstractAssembler の概要の説明です。
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
                if(this.ComponentDef.Container.HasComponentDef(argTypes[i]))
				{
					args[i] = this.ComponentDef.Container.GetComponent(argTypes[i]);
				} 
				else
				{
					logger_.Log("WSSR0007",
						new object[] {this.ComponentDef.ComponentType.FullName});
					args[i] = null;
				}
			}
			return args;
		}

        /// <summary>
        /// 受け側のTypeを指定してコンポーネントを取得する
        /// </summary>
        /// <param name="receiveType">受け側のType</param>
        /// <param name="expression">expression</param>
        /// <returns>expressionからコンポーネント定義を探す</returns>
        protected object GetComponentByReceiveType(Type receiveType, string expression)
        {
            IS2Container container = this.ComponentDef.Container;
            object value = null;

            if (AutoBindingUtil.IsSuitable(receiveType) && expression != null
                && container.HasComponentDef(expression))
            {
                IComponentDef cd = container.GetComponentDef(expression);
                value = cd.GetComponent(receiveType);
            }

            return value;
        }
	}
}
