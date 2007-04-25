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
    /// コンポーネントのインスタンスをS2コンテナ上でどのように管理するのかを定義します。
    /// </summary>
    /// <remarks>
    /// <para>インスタンス定義の種類には、以下のものがあります。</para>
    /// <list type="bullet">
    /// <item>
    /// <term><code>singleton</code>(default)</term>
    /// <description>S2コンテナ上で唯一のインスタンスになります。</description>
    /// </item>
    /// <item>
    /// <term><code>prototype</code></term>
    /// <description>コンポーネントが必要とされる度に異なるインスタンスになります。</description>
    /// </item>
    /// <item>
    /// <term><code>application</code></term>
    /// <description>アプリケーションコンテキスト毎に1つのインスタンスになります。</description>
    /// </item>
    /// <item>
    /// <term><code>request</code></term>
    /// <description>リクエストコンテキスト毎に1つのインスタンスになります。</description>
    /// </item>
    /// <item>
    /// <term><code>session</code></term>
    /// <description>セッションコンテキスト毎に1つのインスタンスになります</description>
    /// </item>
    /// <item>
    /// <term><code>outer</code></term>
    /// <description>
    /// コンポーネントのインスタンスは<see cref="IS2Container"/>の外で生成し、
    /// インジェクションだけを行ないます。アスペクト、コンストラクタ・インジェクションは適用できません。
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// それぞれ、 インスタンスが生成されるタイミングは、そのコンポーネントが必要とされる時になります。
    /// また、その時点で存在する「コンテキスト」に属するコンポーネントのみインジェクションが可能です。
    /// </para>
    /// <para>インスタンス定義を省略した場合は<code>singleton</code>を指定したことになります。</para>
    /// </remarks>
    public interface IInstanceDef
    {
        /// <summary>
        /// インスタンス定義の文字列表現を返します。
        /// </summary>
        /// <value>インスタンス定義を表す文字列</value>
        string Name { get; }

        /// <summary>
        /// インスタンス定義に基づいた、コンポーネント定義<code>componentDef</code>の
        /// <see cref="IComponentDeployer"/>を返します。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <returns><see cref="IComponentDeployer"/>オブジェクト</returns>
        IComponentDeployer CreateComponentDeployer(IComponentDef componentDef);
    }
}
