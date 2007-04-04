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
using System.Collections;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// 式を表わすインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 式とは、 オブジェクトの生成、 プロパティへのアクセス、 メソッドの呼び出し、 
    /// 定義済みオブジェクトの指定、 リテラルの記述、演算、などが出来る表現方法です。
    /// また、 式の実装によっては(JScript.NETでは)変数の使用なども出来ます。
    /// </para>
    /// <para>
    /// diconファイルの<code>&lt;property&gt;</code>、 <code>&lt;component&gt;</code>、
    /// <code>&lt;initMethod&gt;</code>、 <code>&lt;destroyMethod&gt;</code>、
    /// <code>&lt;arg&gt;</code>、 <code>&lt;meta&gt;</code>、 に式を記述することが出来ます。
    /// </para>
    /// <para>
    /// 定義済みオブジェクトは、 以下のものがあります。
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.ContainerConstants.CONTAINER_NAME" /></term>
    /// <description>現在のdiconファイルを処理しているS2コンテナです。</description>
    /// </item>
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.ContainerConstants.REQUEST_NAME" /></term>
    /// <description>Webコンテナなどで実行されている場合、 
    /// 現在のスレッドで処理しているリクエストです。</description>
    /// </item>
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.ContainerConstants.RESPONSE_NAME" /></term>
    /// <description>Webコンテナなどで実行されている場合、
    /// 現在のスレッドで処理しているレスポンスです。</description>
    /// </item>
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.ContainerConstants.SESSION_NAME" /></term>
    /// <description>Webコンテナなどで実行されている場合、
    /// 現在のスレッドで処理しているセッションです。</description>
    /// </item>
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.ContainerConstants.APPLICATION_CONTEXT_NAME" /></term>
    /// <description>Webコンテナなどで実行されている場合、
    /// 現在のS2コンテナに関連づけられたアプリケーションコンテキストです。</description>
    /// </item>
    /// </list>
    /// <para>定義済みオブジェクトの他にも、 S2コンテナに登録されているコンポーネントを
    /// <code>name</code>属性で参照することが出来ます。</para>
    /// </remarks>
    public interface IExpression
    {
        /// <summary>
        /// 式を評価した結果を返します。
        /// </summary>
        /// <param name="container">式を評価するコンテキストとなるS2コンテナ</param>
        /// <param name="context">式を評価するコンテキストに追加できるコンテキスト</param>
        /// <returns>式を評価した結果</returns>
        object Evaluate(IS2Container container, IDictionary context);
    }
}
