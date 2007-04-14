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
    /// アクセスタイプ定義で使用される定数を定義するクラスです。
    /// </summary>
    public class AccessTypeDefConstants
    {
        /// <summary>
        /// アクセスタイプ定義<see cref="Seasar.Framework.Container.Assembler.IAccessTypePropertyDef">property</see>
        /// を表す定数です。
        /// </summary>
        public const string PROPERTY_NAME = "property";

        /// <summary>
        /// アクセスタイプ定義<see cref="Seasar.Framework.Container.Assembler.IAccessTypePropertyDef">field</see>
        /// を表す定数です。
        /// </summary>
        public const string FIELD_NAME = "field";
    }
}
