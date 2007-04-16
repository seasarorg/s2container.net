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
    /// 不正なバインディングタイプ定義が指定された場合にスローされます。
    /// </summary>
    /// <seealso cref="IBindingTypeDef"/>
    /// <seealso cref="Seasar.Framework.Container.Assembler.BindingTypeDefFactory"/>
    [Serializable]
    public class IllegalBindingTypeDefRuntimeException : SRuntimeException
    {
        private string bindingTypeName;

        /// <summary>
        /// <code>IllegalBindingTypeDefRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="bindingTypeName">指定された不正なバインディングタイプ定義名</param>
        public IllegalBindingTypeDefRuntimeException(string bindingTypeName)
            : base("ESSR0079", new object[] { bindingTypeName })
        {
            this.bindingTypeName = bindingTypeName;
        }

        /// <summary>
        /// 例外の原因となった不正なバインディングタイプ定義名を返します。
        /// </summary>
        /// <value>バインディングタイプ定義名</value>
        public string BindingTypeName
        {
            get { return bindingTypeName; }
        }
    }
}
