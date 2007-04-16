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
    /// 不正なアクセスタイプ定義が指定された場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 有効な<see cref="IAccessTypeDef">アクセスタイプ定義</see>としては、
    /// <see cref="Seasar.Framework.Container.Assembler.AccessTypePropertyDef">PROPERTY</see>と
    /// <see cref="Seasar.Framework.Container.Assembler.AccessTypeFieldDef">FIELD</see>があります。
    /// </para>
    /// </remarks>
    /// <seealso cref="Seasar.Framework.Container.Assembler.AccessTypeDefFactory.GetAccessTypeDef"/>
    [Serializable]
    public class IllegalAccessTypeDefRuntimeException : SRuntimeException
    {
        private string accessTypeName;

        /// <summary>
        /// 不正なアクセスタイプ定義名を指定して、
        /// <code>IllegalAccessTypeDefRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="accessTypeName">不正なアクセスタイプ定義名</param>
        public IllegalAccessTypeDefRuntimeException(string accessTypeName)
            : base("ESSR0083", new object[] { accessTypeName })
        {
            this.accessTypeName = accessTypeName;
        }

        /// <summary>
        /// 不正なアクセスタイプ定義名を返します。
        /// </summary>
        /// <value>不正なアクセスタイプ定義名</value>
        /// <seealso cref="IAccessTypeDefConstants.PROPERTY_NAME"/>
        /// <seealso cref="IAccessTypeDefConstants.FIELD_NAME"/>
        public string AccessTypeName
        {
            get { return accessTypeName; }
        }
    }
}
