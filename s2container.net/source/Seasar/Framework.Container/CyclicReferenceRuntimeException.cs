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
    /// コンポーネントの循環参照が発生した場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>コンポーネントのコンストラクタ引数に、 同じコンポーネントを指定した場合などに発生します。</para>
    /// </remarks>
    [Serializable]
    public class CyclicReferenceRuntimeException : SRuntimeException
    {
        private Type componentType;

        /// <summary>
        /// 循環参照を引き起こしたコンポーネントのクラスを指定して、
        /// <code>CyclicReferenceRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="componentType">循環参照を引き起こしたコンポーネントのクラスのType</param>
        public CyclicReferenceRuntimeException(Type componentType)
            : base("ESSR0047", new object[] { componentType.FullName })
        {
            this.componentType = componentType;
        }

        /// <summary>
        /// 循環参照を引き起こしたコンポーネントのクラスのTypeを返します。
        /// </summary>
        /// <value>循環参照を引き起こしたコンポーネントのクラスのType</value>
        public Type ComponentType
        {
            get { return componentType; }
        }
    }
}
