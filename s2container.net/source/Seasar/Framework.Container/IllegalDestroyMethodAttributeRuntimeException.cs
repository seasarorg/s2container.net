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
    /// 属性で指定された<see cref="IDestroyMethodDef">destroyメソッド・インジェクション定義</see>
    /// が不正だった場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 属性で指定されたメソッドが存在しない場合、複数定義されている場合、
    /// および引数が必要な場合に不正とみなされます。
    /// </para>
    /// </remarks>
    [Serializable]
    public class IllegalDestroyMethodAttributeRuntimeException : SRuntimeException
    {
        private Type componentType;
        private string methodName;

        /// <summary>
        /// <code>IllegalDestroyMethodAnnotationRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="componentType">属性が指定されたクラス</param>
        /// <param name="methodName">属性で指定されたメソッド名</param>
        public IllegalDestroyMethodAttributeRuntimeException(
            Type componentType, string methodName)
            : base("ESSR0082", new object[] { componentType.FullName, methodName })
        {
            this.componentType = componentType;
            this.methodName = methodName;
        }

        /// <summary>
        /// 例外の原因となった属性が指定されたクラスを返します。
        /// </summary>
        /// <value>属性が指定されたクラス</value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// 例外の原因となった属性で指定されたメソッド名を返します。
        /// </summary>
        /// <value>属性で指定されたメソッド名</value>
        public string MethodName
        {
            get { return methodName; }
        }
    }
}
