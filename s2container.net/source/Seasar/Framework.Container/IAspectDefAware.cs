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
    /// このインターフェースは、アスペクト定義を登録および取得することができるオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// アスペクト定義は複数登録することが出来ます。
    /// アスペクト定義の取得はインデックス番号を指定して行います。
    /// アスペクト定義は登録されている順に適用されます。
    /// </para>
    /// </remarks>
    /// <seealso cref="IAspectDef"/>
    public interface IAspectDefAware
    {
        /// <summary>
        /// アスペクト定義を追加します。
        /// </summary>
        /// <param name="aspectDef">アスペクト定義</param>
        void AddAspectDef(IAspectDef aspectDef);

        /// <summary>
        /// アスペクト定義を指定の位置に追加します。
        /// </summary>
        /// <param name="index">アスペクト定義を追加する位置</param>
        /// <param name="aspectDef">アスペクト定義</param>
        void AddAspectDef(int index, IAspectDef aspectDef);

        /// <summary>
        /// 登録されている<see cref="IAspectDef">アスペクト定義</see>の数を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 登録されている<see cref="Seasar.Framework.Aop.IMethodInterceptor">インターセプタ</see>
        /// の数ではなく、アスペクト定義の数を返します。
        /// アスペクト定義のコンポーネント(インターセプタ)のクラスが
        /// <see cref="Seasar.Framework.Aop.Interceptors.InterceptorChain"/>で、
        /// その中に複数のインターセプタが含まれる場合も、 1つのアスペクト定義としてカウントします。
        /// </para>
        /// </remarks>
        /// <value>登録されているアスペクト定義の数</value>
        int AspectDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>のアスペクト定義を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// インデックス番号は、 登録した順番に 0,1,2,… となります。
        /// </para>
        /// </remarks>
        /// <param name="index">アスペクト定義を指定するインデックス番号</param>
        /// <returns>アスペクト定義</returns>
        IAspectDef GetAspectDef(int index);
    }
}
