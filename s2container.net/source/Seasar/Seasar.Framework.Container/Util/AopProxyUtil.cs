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
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Framework.Container.Util
{
	/// <summary>
	/// AopProxyUtil の概要の説明です。
	/// </summary>
	public sealed class AopProxyUtil
	{
        /// <summary>
        /// デフォルトのAspectWeaverインスタンス
        /// </summary>
        private static IAspectWeaver DEFAULT_ASPECTWEAVER_INSTANCE = new AopProxyAspectWeaver();

		private AopProxyUtil()
		{
		}

        /// <summary>
        /// Aspectを織り込む
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
		public static void WeaveAspect(ref object target,IComponentDef componentDef)
		{
            // S2コンテナを取得する
            IS2Container container = componentDef.Container;

            // S2コンテナにIAspectWeaverが存在する場合は、S2コンテナから取得する
            // 存在しない場合は、デフォルトのAopProxyAspectWeaverを使用する
            IAspectWeaver aspectWeaver = container.HasComponentDef(typeof(IAspectWeaver)) ?
                (IAspectWeaver) container.GetComponent(typeof(IAspectWeaver)) : DEFAULT_ASPECTWEAVER_INSTANCE;

            // Aspectを織り込む
            aspectWeaver.WeaveAspect(ref target, componentDef);

		}
	}
}
