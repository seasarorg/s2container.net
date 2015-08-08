#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// コンポーネントを自動登録する基底クラスです。
    /// </summary>
    public abstract class AbstractComponentAutoRegister : AbstractAutoRegister
    {
        /// <summary>
        /// コンポーネントに自動的に名前を付ける為の AutoNaming を設定・取得します。
        /// </summary>
        public IAutoNaming AutoNaming { set; get; } = new DefaultAutoNaming();

        /// <summary>
        /// インスタンスのモードを設定・取得します。
        /// </summary>
        public string InstanceMode { set; get; } = ContainerConstants.INSTANCE_SINGLETON;

        /// <summary>
        /// 自動バインディングのモードを設定・取得します。
        /// </summary>
        public string AutoBindingMode { set; get; } = ContainerConstants.AUTO_BINDING_AUTO;

        /// <summary>
        /// Type に対して自動登録するかどうかの処理を行います。
        /// </summary>
        /// <param name="type">自動登録を行うか判断する対象の Type</param>
        public void ProcessType(Type type)
        {
            if (IsIgnore(type))
            {
                return;
            }

            for (var i = 0; i < ClassPatternSize; ++i)
            {
                var cp = GetClassPattern(i);

                if (cp.IsAppliedNamespaceName(type.Namespace)
                    && cp.IsAppliedShortClassName(type.Name))
                {
                    Register(type);
                    return;
                }
            }
        }

        /// <summary>
        /// コンポーネントを登録します。
        /// </summary>
        /// <param name="type">登録する Type</param>
        public void Register(Type type)
        {
            var componentName = AutoNaming.DefineName(type);
            IComponentDef cd = new ComponentDefImpl(type, componentName);
            cd.InstanceMode = InstanceMode;
            cd.AutoBindingMode = AutoBindingMode;

            Container.Register(cd);
        }
    }
}
