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
    /// S2コンテナ上で、 Webコンテナなどの外部コンテキストを扱うためのインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IInstanceDef">インスタンス定義</see>で、<code>application</code>、
    /// <code>request</code>、 <code>session</code>を使う場合、
    /// <see cref="IS2Container.Init"/>を行う前に
    /// <code>IExternalContext</code>をS2コンテナに設定する必要があります。
    /// </para>
    /// </remarks>
    public interface IExternalContext
    {
        /// <summary>
        /// リクエストコンテキストを取得・設定します。
        /// </summary>
        /// <value>リクエストコンテキスト</value>
        /// <seealso cref="InstanceDefConstants.REQUEST_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceRequestDef"/>
        object Request { get; set; }

        /// <summary>
        /// レスポンスコンテキストを取得・設定します。
        /// </summary>
        /// <value>レスポンスコンテキスト</value>
        object Response { get; set; }

        /// <summary>
        /// セッションコンテキストを返します。
        /// </summary>
        /// <value>セッションコンテキスト</value>
        /// <see cref="InstanceDefConstants.SESSION_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceSessionDef"/>
        object Session { get; }

        /// <summary>
        /// アプリケーションコンテキストを取得・設定します。
        /// </summary>
        /// <value>アプリケーションコンテキスト</value>
        /// <see cref="InstanceDefConstants.APPLICATION_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceApplicationDef"/>
        object Application { get; set; }

        /// <summary>
        /// アプリケーションコンテキストを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで返します。
        /// </summary>
        /// <value>アプリケーションコンテキスト</value>
        /// <see cref="InstanceDefConstants.APPLICATION_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceApplicationDef"/>
        IDictionary ApplicationDictionary { get; }

        /// <summary>
        /// 初期設定値を<see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>初期設定値</value>
        IDictionary InitParameterDictionary { get; }

        /// <summary>
        /// セッションコンテキストを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで返します。
        /// </summary>
        /// <value>セッションコンテキスト</value>
        /// <see cref="InstanceDefConstants.SESSION_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceSessionDef"/>
        IDictionary SessionDictionary { get; }

        /// <summary>
        /// リクエストクッキーを<see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>リクエストクッキー</value>
        IDictionary RequestCookieDictionary { get; }

        /// <summary>
        /// キーに対する値を1つ持つリクエストヘッダーを
        /// <see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>キーに対する値を1つ持つリクエストヘッダー</value>
        IDictionary RequestHeaderDictionary { get; }

        /// <summary>
        /// キーに対する値を複数持つリクエストヘッダーを
        /// <see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>キーに対する値を複数持つリクエストヘッダー</value>
        IDictionary RequestHeaderValuesDictionary { get; }

        /// <summary>
        /// リクエストコンテキストを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで返します。
        /// </summary>
        /// <value>リクエストコンテキスト</value>
        /// <seealso cref="InstanceDefConstants.REQUEST_NAME"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceRequestDef"/>
        IDictionary RequestDictionary { get; }

        /// <summary>
        /// キーに対する値を1つ持つリクエストパラメータを
        /// <see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>キーに対する値を1つ持つリクエストパラメータ</value>
        IDictionary RequestParameterDictionary { get; }

        /// <summary>
        /// キーに対する値を複数持つリクエストパラメータを
        /// <see cref="System.Collections.IDictionary"/>インターフェースで返します。
        /// </summary>
        /// <value>キーに対する値を複数持つリクエストパラメータ</value>
        IDictionary RequestParameterValuesDictionary { get; }
    }
}
