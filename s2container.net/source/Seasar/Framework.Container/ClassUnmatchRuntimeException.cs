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
    /// コンポーネントのインスタンスを、 <see cref="IComponentDef">コンポーネント定義</see>
    /// に指定されたクラスにキャスト出来ない場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IComponentDef.Expression"/>でインスタンスの生成を定義している場合は、
    /// そのインスタンスをコンポーネント定義に指定されたクラスにキャスト出来ないことを表します。
    /// </para>
    /// <para>
    /// 外部コンポーネントを<see cref="IS2Container.InjectDependency"/>などでインジェクションする場合は、
    /// そのコンポーネントを、 コンポーネント定義に指定されたクラスにキャストできないことを表します。
    /// </para>
    /// </remarks>
    /// <see cref="Seasar.Framework.Container.IConstructorAssembler.Assemble"/>
    /// <see cref="Seasar.Framework.Container.IS2Container.InjectDependency"/>
    [Serializable]
    public class ClassUnmatchRuntimeException : SRuntimeException
    {
        private Type componentType;
        private Type realComponentType;

        /// <summary>
        /// <code>ClassUnmatchRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="componentType">コンポーネント定義に指定されたクラスのType</param>
        /// <param name="realComponentType">コンポーネントの実際の型</param>
        public ClassUnmatchRuntimeException(Type componentType, Type realComponentType)
            : base("ESSR0069", new object[] { componentType.FullName,
                realComponentType != null ? realComponentType.FullName : "null" })
        {
            this.componentType = componentType;
            this.realComponentType = realComponentType;
        }

        /// <summary>
        /// コンポーネント定義に指定されたクラスのTypeを返します。
        /// </summary>
        /// <value>コンポーネント定義に指定されたクラスのType</value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// コンポーネントの実際の型を返します。
        /// </summary>
        /// <value>コンポーネントの実際の型</value>
        public Type RealComponentType
        {
            get { return realComponentType; }
        }
    }
}
