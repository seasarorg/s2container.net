#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
    /// コンポーネントに自動的に名前を付ける為の標準のクラスです。
    /// </summary>
    public class DefaultAutoNaming : IAutoNaming
    {
        #region IAutoNaming メンバ

        /// <summary>
        /// コンポーネント名を定義します。
        /// </summary>
        /// <remarks>
        /// <para>インターフェースの場合はプリフィックスの"I"を取り除いた
        /// インターフェース名（名前空間は含まない）をコンポーネント名とします。
        /// （ただし2文字目が大文字の場合のみ1文字目をプリフィックスと判断します）</para>
        /// <para>それ以外は名前空間を含まない簡易名をコンポーネント名とします。</para>
        /// </remarks>
        /// <param name="type">コンポーネント名を定義したいType</param>
        /// <returns>コンポーネント名</returns>
        public string DefineName(Type type)
        {
            string name;

            if (type.IsInterface && type.Name.Length > 1 && type.Name[0] == 'I'
                && char.IsUpper(type.Name[1]))
            {
                // インターフェースのプリフィックスがあると判断した場合は
                // プリフィックス"I"を除いたものをコンポーネント名とする
                name = type.Name.Substring(1);
            }
            else
            {
                // 上記以外の場合は簡易名をコンポーネント名とする
                name = type.Name;
            }

            return name;
        }

        #endregion
    }
}
