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
    /// インスタンス定義で使用される定数を定義するクラスです。
    /// </summary>
    public class InstanceDefConstants
    {
        /// <summary>
        /// インスタンス定義「<code>singleton</code>」を表す定数です。
        /// </summary>
        public const string SINGLETON_NAME = "singleton";

        /// <summary>
        /// インスタンス定義「<code>prototype</code>」を表す定数です。
        /// </summary>
        public const string PROTOTYPE_NAME = "prototype";

        /// <summary>
        /// インスタンス定義「<code>application</code>」を表す定数です。
        /// </summary>
        public const string APPLICATION_NAME = "application";

        /// <summary>
        /// インスタンス定義「<code>request</code>」を表す定数です。
        /// </summary>
        public const string REQUEST_NAME = "request";

        /// <summary>
        /// インスタンス定義「<code>session</code>」を表す定数です。
        /// </summary>
        public const string SESSION_NAME = "session";

        /// <summary>
        /// インスタンス定義「<code>outer</code>」を表す定数です。
        /// </summary>
        public const string OUTER_NAME = "outer";
    }
}
