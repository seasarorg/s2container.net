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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// コンポーネントの構築に失敗した場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// この例外は、 コンポーネント定義でコンストラクタの引数として
    /// 指定されたコンポーネントの取得に失敗した場合などに発生します。
    /// </para>
    /// </remarks>
    [Serializable]
    public class IllegalConstructorRuntimeException : SRuntimeException
    {
        private Type componentType;

        /// <summary>
        /// <code>IllegalConstructorRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="componentType">構築に失敗したコンポーネントのクラスのType</param>
        /// <param name="cause">コンポーネントの構築に失敗した原因を表すエラーまたは例外</param>
        public IllegalConstructorRuntimeException(Type componentType, Exception cause)
            : base("ESSR0058", new object[] { componentType.FullName, cause }, cause)
        {
            this.componentType = componentType;
        }

        /// <summary>
        /// 構築に失敗したコンポーネントのクラスを返します。
        /// </summary>
        /// <value>構築に失敗したコンポーネントのクラス</value>
        public Type ComponentType
        {
            get { return componentType; }
        }
    }
}
