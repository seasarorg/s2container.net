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
using Seasar.Framework.Container.Util;
using Seasar.Framework.Log;

namespace Seasar.Framework.Container.Assembler
{
    public abstract class AbstractAssembler
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IComponentDef _componentDef;

        public AbstractAssembler(IComponentDef componentDef)
        {
            _componentDef = componentDef;
        }

        protected IComponentDef ComponentDef
        {
            get { return _componentDef; }
        }

        protected Type GetComponentType()
        {
            return _componentDef.ComponentType;
        }

        protected Type GetComponentType(object component)
        {
            Type type = _componentDef.ComponentType;
            if (type != null)
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
            for (int i = 0; i < argTypes.Length; i++)
            {
                if (ComponentDef.Container.HasComponentDef(argTypes[i]))
                {
                    args[i] = ComponentDef.Container.GetComponent(argTypes[i]);
                }
                else
                {
                    _logger.Log("WSSR0007",
                        new object[] { ComponentDef.ComponentType.FullName });
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
            IS2Container container = ComponentDef.Container;
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
