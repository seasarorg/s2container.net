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
    /// このインターフェースは、<see cref="IArgDef">引数定義</see>
    /// を登録および取得することができるオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 引数定義は複数登録することが出来ます。 
    /// 引数定義の取得はインデックス番号を指定して行います。
    /// </para>
    /// </remarks>
    public interface IArgDefAware
    {
        /// <summary>
        /// 引数定義を追加します。
        /// </summary>
        /// <param name="argDef">引数定義</param>
        void AddArgDef(IArgDef argDef);

        /// <summary>
        /// 登録されている<see cref="IArgDef">引数定義</see>の数を返します。
        /// </summary>
        /// <value>登録されている引数定義の数</value>
        int ArgDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>の引数定義を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// インデックス番号は、 登録した順番に 0,1,2,… となります。
        /// </para>
        /// </remarks>
        /// <param name="index">引数定義を指定するインデックス番号</param>
        /// <returns>引数定義</returns>
        IArgDef GetArgDef(int index);
    }
}
