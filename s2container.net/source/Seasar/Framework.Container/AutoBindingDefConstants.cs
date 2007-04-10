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
    /// 自動バインディング定義で使用される定数を定義するクラスです。
    /// </summary>
    public class AutoBindingDefConstants
    {
        /// <summary>
        /// 自動バインディング定義名「<code>auto</code>」を表す定数です。
        /// </summary>
        public const string AUTO_NAME = "auto";

        /// <summary>
        /// 自動バインディング定義名「<code>semiauto</code>」を表す定数です。
        /// </summary>
        public const string SEMIAUTO_NAME = "semiauto";

        /// <summary>
        /// 自動バインディング定義名「<code>constructor</code>」を表す定数です。
        /// </summary>
        public const string CONSTRUCTOR_NAME = "constructor";

        /// <summary>
        /// 自動バインディング定義名「<code>property</code>」を表す定数です。
        /// </summary>
        public const string PROPERTY_NAME = "property";

        /// <summary>
        /// 自動バインディング定義名「<code>none</code>」を表す定数です。
        /// </summary>
        public const string NONE_NAME = "none";
    }
}
