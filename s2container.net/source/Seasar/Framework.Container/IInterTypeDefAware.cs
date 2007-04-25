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

namespace Seasar.Framework.Container
{
    /// <summary>
    /// このインターフェースはインタータイプ定義を登録および取得する方法を
    /// 定義するオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// インタータイプ定義は複数登録することが出来ます。 
    /// インタータイプ定義の取得はインデックス番号を指定して行います。
    /// </para>
    /// </remarks>
    /// <seealso cref="IInterTypeDef"/>
    public interface IInterTypeDefAware
    {
        /// <summary>
        /// <see cref="IInterTypeDef">インタータイプ定義</see>を追加します。
        /// </summary>
        /// <param name="interTypeDef">インタータイプ定義</param>
        void AddInterTypeDef(IInterTypeDef interTypeDef);

        /// <summary>
        /// 登録されている<see cref="IInterTypeDef">インタータイプ定義</see>の数を返します。
        /// </summary>
        /// <value>登録されているインタータイプ定義の数</value>
        int InterTypeDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>の
        /// <see cref="IInterTypeDef">インタータイプ定義</see>を返します。
        /// <see cref="IInterTypeDef">
        /// </summary>
        /// <param name="index">インタータイプ定義を指定するインデックス番号</param>
        /// <returns>インタータイプ定義</returns>
        IInterTypeDef GetInterTypeDef(int index);
    }
}
