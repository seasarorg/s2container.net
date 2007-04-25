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
    /// このインターフェースは、initメソッド定義を登録および取得することができるオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// initメソッド定義は複数登録することが出来ます。
    /// initメソッド定義の取得はインデックス番号を指定して行います。
    /// </para>
    /// </remarks>
    /// <seealso cref="IInitMethodDef"/>
    public interface IInitMethodDefAware
    {
        /// <summary>
        /// initメソッド定義を追加します。
        /// </summary>
        /// <param name="methodDef">initメソッド定義</param>
        void AddInitMethodDef(IInitMethodDef methodDef);

        /// <summary>
        /// 登録されている<see cref="IInitMethodDef">initメソッド定義</see>の数を返します。
        /// </summary>
        /// <value>登録されているinitメソッド定義の数</value>
        int InitMethodDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>のinitメソッド定義を返します。
        /// </summary>
        /// <remarks>
        /// <para>インデックス番号は、 登録した順番に 0,1,2,… となります。</para>
        /// </remarks>
        /// <param name="index">initメソッド定義を指定するインデックス番号</param>
        /// <returns>initメソッド定義</returns>
        IInitMethodDef GetInitMethodDef(int index);

    }
}
