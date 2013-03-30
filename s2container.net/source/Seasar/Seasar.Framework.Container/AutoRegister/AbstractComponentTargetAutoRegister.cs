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

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// コンポーネントを対象にした自動登録を行うための抽象クラスです。
    /// </summary>
    public abstract class AbstractComponentTargetAutoRegister : AbstractAutoRegister
    {
        /// <summary>
        /// コンポーネントを対象にして自動登録を行います。
        /// </summary>
        public override void RegisterAll()
        {
            IS2Container container = Container;

            for (int i = 0; i < container.ComponentDefSize; ++i)
            {
                IComponentDef cd = container.GetComponentDef(i);

                if (IsAppliedComponent(cd))
                {
                    Register(cd);
                }
            }
        }

        /// <summary>
        /// <see cref="IComponentDef"/>を登録します。
        /// </summary>
        /// <param name="cd"></param>
        protected abstract void Register(IComponentDef cd);

        /// <summary>
        /// 処理対象のコンポーネントかどうかを返します。
        /// </summary>
        /// <param name="cd">コンポーネント定義</param>
        /// <returns>処理対象のコンポーネントかどうか</returns>
        protected bool IsAppliedComponent(IComponentDef cd)
        {
            Type componentType = cd.ComponentType;

            if (componentType == null)
            {
                return false;
            }

            string namespaceName = componentType.Namespace;
            string shortClassName = componentType.Name;

            for (int i = 0; i < ClassPatternSize; ++i)
            {
                ClassPattern cp = GetClassPattern(i);

                if (IsIgnore(componentType))
                {
                    return false;
                }

                if (cp.IsAppliedNamespaceName(namespaceName)
                    && cp.IsAppliedShortClassName(shortClassName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
