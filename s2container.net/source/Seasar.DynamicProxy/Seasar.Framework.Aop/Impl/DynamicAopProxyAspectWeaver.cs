#region Copyright
/*
* Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Reflection;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Container;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// DynamicAopProxyを用いてAspectを織り込む処理を持つクラス
    /// </summary>
    public class DynamicAopProxyAspectWeaver : AbstractAspectWeaver
    {
        private readonly IDictionary<IComponentDef, DynamicAopProxy> _aopProxies =
            new Dictionary<IComponentDef, DynamicAopProxy>();

        /// <summary>
        /// AopProxyを用いてAspectを織り込む
        /// </summary>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        public override object WeaveAspect(IComponentDef componentDef, ConstructorInfo constructor, object[] args)
        {
            object target = null;
            if (componentDef.AspectDefSize == 0)
            {
                target = ConstructorUtil.NewInstance(constructor, args);
                return target;
            }

            if (!componentDef.ComponentType.IsInterface)
            {
                DynamicAopProxy aopProxy = GetAopProxy(target, componentDef);
                target = aopProxy.Create(Type.GetTypeArray(args), args);
            }
            else
            {
                target = new object();
                AddProxy(target, componentDef, componentDef.ComponentType);
            }

            Type[] interfaces = componentDef.ComponentType.GetInterfaces();

            foreach (Type interfaceType in interfaces)
            {
                AddProxy(target, componentDef, interfaceType);
            }

            return target;
        }

        /// <summary>
        /// コンポーネント定義にProxyを追加する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="type">コンポーネント定義に追加するProxyのType</param>
        protected void AddProxy(object target, IComponentDef componentDef, Type type)
        {
            DynamicAopProxy aopProxy = GetAopProxy(target, componentDef);

            componentDef.AddProxy(type, aopProxy.Create(type, target));
        }

        /// <summary>
        /// Proxyを作成する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <returns>Proxy</returns>
        protected DynamicAopProxy GetAopProxy(object target, IComponentDef componentDef)
        {
            DynamicAopProxy aopProxy;

            if (_aopProxies.ContainsKey(componentDef))
            {
                aopProxy = _aopProxies[componentDef];
            }
            else
            {
                Hashtable parameters = new Hashtable();
                parameters[ContainerConstants.COMPONENT_DEF_NAME] = componentDef;
                aopProxy = new DynamicAopProxy(componentDef.ComponentType,
                    GetAspects(componentDef), parameters, target);
                _aopProxies[componentDef] = aopProxy;
            }

            return aopProxy;
        }
    }
}
