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
    /// 不正なメソッド・インジェクション定義が指定されていた場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// メソッド・インジェクションを実行した際に、メソッドの引数として指定されたコンポーネントが
    /// 見つからない場合や、引数を適切な型に変換出来ない場合などに発生します。
    /// </para>
    /// </remarks>
    /// <seealso cref="IMethodDef"/>
    /// <seealso cref="IInitMethodDef"/>
    /// <seealso cref="IDestroyMethodDef"/>
    /// <seealso cref="IMethodAssembler"/>
    /// <seealso cref="Seasar.Framework.Container.Assembler.AbstraceMethodAssembler"/>
    [Serializable]
    public class IllegalMethodRuntimeException : SRuntimeException
    {
        private Type componentType;
        private string methodName;

        public IllegalMethodRuntimeException(Type componentType,
            string methodName, Exception cause)
            : base("ESSR0060", new object[] { componentType.FullName, 
                methodName, cause }, cause)
        {
            this.componentType = componentType;
            this.methodName = methodName;
        }

        /// <summary>
        /// 不正なメソッド・インジェクション定義を含むコンポーネントのクラスのTypeを返します。
        /// </summary>
        /// <value>コンポーネントのクラスのType</value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// 不正なメソッド・インジェクション定義のメソッド名を返します。
        /// </summary>
        /// <value>メソッド名</value>
        public string MethodName
        {
            get { return methodName; }
        }
    }
}
