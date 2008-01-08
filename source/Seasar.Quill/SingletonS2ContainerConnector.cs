#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Container;

namespace Seasar.Quill
{
    /// <summary>
    /// S2Containerと連携する為の静的クラスです
    /// </summary>
    /// <remarks>
    /// <see cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory"/>
    /// で作成された<see cref="Seasar.Framework.Container.IS2Container"/>を扱います
    /// </remarks>
    public static class SingletonS2ContainerConnector
    {
        /// <summary>
        /// S2Containerのコンポーネントをコンポーネント名を指定して取得します
        /// </summary>
        /// <remarks>
        /// see cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory"/>
        /// で作成された<see cref="Seasar.Framework.Container.IS2Container"/>
        /// からコンポーネントを取得します
        /// </remarks>
        /// <param name="componentName">コンポーネント名</param>
        /// <returns>コンポーネントのインスタンス</returns>
        public static object GetComponent(string componentName)
        {
            // S2Containerのコンポーネントをコンポーネント名を指定して取得します
            return GetComponent(componentName, null);
        }

        /// <summary>
        /// S2Containerのコンポーネントをコンポーネント名を指定して取得します
        /// </summary>
        /// <remarks>
        /// see cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory"/>
        /// で作成された<see cref="Seasar.Framework.Container.IS2Container"/>
        /// からコンポーネントを取得します
        /// </remarks>
        /// <param name="componentName">コンポーネント名</param>
        /// <param name="receiptType">受け側のType</param>
        /// <returns>コンポーネントのインスタンス</returns>
        public static object GetComponent(string componentName, Type receiptType)
        {
            if (!SingletonS2ContainerFactory.HasContainer)
            {
                // S2Containerが作成されていない場合は例外をスローします
                throw new QuillApplicationException("EQLL0009");
            }

            // S2Containerを取得する
            IS2Container container = SingletonS2ContainerFactory.Container;

            if (!container.HasComponentDef(componentName))
            {
                // S2Containerにコンポーネントが登録されていない場合は例外をスローする
                throw new QuillApplicationException("EQLL0010",
                    new string[] { componentName });
            }

            try
            {
                if (receiptType == null)
                {
                    // S2Containerから取得したコンポーネントを返す
                    return container.GetComponent(componentName);
                }
                else
                {
                    // 受け側の型を指定してS2Containerから取得したコンポーネントを返す
                    return container.GetComponent(receiptType, componentName);
                }
            }
            catch (Exception ex)
            {
                // コンポーネントの取得で例外が発生した場合は例外をスローする
                throw new QuillApplicationException("EQLL0011", new string[] { }, ex);
            }
        }
    }
}
