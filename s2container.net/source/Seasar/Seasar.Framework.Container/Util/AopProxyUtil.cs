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
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Util
{
    public sealed class AopProxyUtil
    {
        /// <summary>
        /// デフォルトのAspectWeaverインスタンス
        /// </summary>
        private static readonly IAspectWeaver DEFAULT_ASPECTWEAVER_INSTANCE = new AopProxyAspectWeaver();

        private AopProxyUtil()
        {
        }

        /// <summary>
        /// Aspectを織り込む
        /// </summary>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        public static object WeaveAspect(IComponentDef componentDef, ConstructorInfo constructor, object[] args)
        {
            // S2コンテナを取得する
            IS2Container container = componentDef.Container;

            if (componentDef.ComponentType.FindInterfaces(new TypeFilter(TypeFilter), null).Length > 0)
            {
                return ConstructorUtil.NewInstance(constructor, args);
            }

            // S2コンテナにIAspectWeaverが存在する場合は、S2コンテナから取得する
            // 存在しない場合は、デフォルトのAopProxyAspectWeaverを使用する
            IAspectWeaver aspectWeaver = container != null && container.HasComponentDef(typeof(IAspectWeaver)) ?
                (IAspectWeaver) container.GetComponent(typeof(IAspectWeaver)) : DEFAULT_ASPECTWEAVER_INSTANCE;

            // Aspectを織り込む
            return aspectWeaver.WeaveAspect(componentDef, constructor, args);

        }

        public static bool TypeFilter(Type type, object filterCriteria)
        {
            return type == typeof(IAspectWeaver);
        }
    }
}
