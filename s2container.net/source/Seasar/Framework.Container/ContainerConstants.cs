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
    /// S2コンテナで使用される定数を定義するクラスです。
    /// </summary>
    /// <remarks>
    /// <para>セパレータ文字や定義済みコンポーネントキー(コンポーネント名)
    /// などの定数を定義しています。</para>
    /// </remarks>
    public class ContainerConstants
    {
        /// <summary>
        /// 名前空間とコンポーネント名の区切り(char)を表す定数です。
        /// </summary>
        public const char NS_SEP = '.';

        /// <summary>
        /// パッケージ名(Javaのパッケージとは異なる)付きコンポーネント名における、
        /// パッケージ名と自動バインディング用コンポーネント名の区切り(char)を表す定数です。
        /// </summary>
        public const char PACKAGE_SEP = '_';

        /// <summary>
        /// 名前空間とコンポーネント名の区切り(String)を表す定数です。
        /// </summary>
        public const string NS_SEP_STR = ".";

        /// <summary>
        /// S2コンテナのコンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.Impl.S2Container"/>
        public const string CONTAINER_NAME = "container";

        /// <summary>
        /// 外部コンテキストが提供するリクエストオブジェクトを取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestComponentDef"/>
        public const string REQUEST_NAME = "request";

        /// <summary>
        /// 外部コンテキストが提供するレスポンスオブジェクトを取得するための、
        /// コンポーネントキーを表す定数です。
        /// </summary>
        public const string RESPONSE_NAME = "response";

        /// <summary>
        /// 外部コンテキストが提供するアプリケーションコンテキストを取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        public const string SESSION_NAME = "session";

        /// <summary>
        /// 外部コンテキストが提供するアプリケーションコンテキストを取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        public const string APPLICATION_CONTEXT_NAME = "application";

        /// <summary>
        /// 外部コンテキストが提供するアプリケーションスコープを
        /// <see cref="System.Collections.IDictionary"/>
        /// インターフェースで取得するための、コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IApplicationComponentDef"/>
        /// <seealso cref="Seasar.Framework.Container.External.IApplicationMapComponentDef"/>
        public const string APPLICATION_SCOPE = "applicationScope";

        /// <summary>
        /// 外部コンテキストの初期化パラメータを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで取得するための、コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IInitParameterMapComponentDef"/>
        public const string INIT_PARAM = "initParam";

        /// <summary>
        /// 外部コンテキストが提供するセッションスコープを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで取得するための、コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.ISessionMapComponentDef"/>
        public const string SESSION_SCOPE = "sessionScope";

        /// <summary>
        /// 外部コンテキストが提供するリクエストコープを<see cref="System.Collections.IDictionary"/>
        /// インターフェースで取得するための、コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestMapComponentDef"/>
        public const string REQUEST_SCOPE = "requestScope";
            
        /// <summary>
        /// 外部コンテキストが提供するクッキー(cookie)の内容を
        /// <see cref="System.Collections.IDictionary"/>インターフェースで取得するための、
        /// コンポーネントキーを表す定数です。
        /// </summary>
        public const string COOKIE = "cookie";

        /// <summary>
        /// 外部コンテキストが提供するリクエストヘッダの内容を
        /// <see cref="System.Collections.IDictionary"/>インターフェースで取得するための、
        /// コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestHeaderMapComponentDef"/>
        public const string HEADER = "header";

        /// <summary>
        /// 外部コンテキストが提供するリクエストヘッダの内容を値の配列で取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestHeaderValuesMapComponentDef"/>
        public const string HEADER_VALUES = "headerValues";

        /// <summary>
        /// 外部コンテキストが提供するリクエストパラメータの内容を
        /// <see cref="System.Collections.IDictionary"/>インターフェースで取得するための、
        /// コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestParameterMapComponentDef"/>
        public const string PARAM = "param";

        /// <summary>
        /// 外部コンテキストが提供するリクエストパラメータの内容を値の配列で取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        /// <seealso cref="Seasar.Framework.Container.External.IRequestParameterValuesMapComponentDef"/>
        public const string PARAM_VALUES = "paramValues";

        /// <summary>
        /// <see cref="Seasar.Framework.Container.IComponentDef"/>(コンポーネント定義)を
        /// <see cref="System.Collections.IDictionary"/>に保持する場合などに、キーとして使用する定数です。
        /// </summary>
        public const string COMPONENT_DEF_NAME = "componentDef";

        /// <summary>
        /// コンフィグレーションオブジェクト(設定情報を内包するオブジェクト)を取得するための、 
        /// コンポーネントキーを表す定数です。
        /// </summary>
        public const string CONFIG_NAME = "config";

        /// <summary>
        /// Propertiesの名前空間を表す定数です。
        /// </summary>
        public const string PROPERTIES_NAMESPACE = "Seasar.Properties";
    }
}
