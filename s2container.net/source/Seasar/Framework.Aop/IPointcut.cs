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

namespace Seasar.Framework.Aop
{
    /// <summary>
    /// ポイントカット
    /// </summary>
    /// <remarks>
    /// <para>どこにJoinpointを設定するのかを定義します。</para>
    /// </remarks>
    public interface IPointcut
    {
        /// <summary>
        /// メソッドにAdvice(MethodInterceptor)を挿入するか確認します
        /// </summary>
        /// <param name="method">メソッドとコンストラクタに関する情報</param>
        /// <returns>
        /// Adviceを挿入する場合、<code>true</code>、
        /// そうでない場合は<code>false</code>を返します。
        /// </returns>
        bool IsApplied(MethodBase method);
    }
}
