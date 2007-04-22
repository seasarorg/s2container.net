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
    /// 自動バインディングの対象となるコンポーネントが見つからなかった場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// この例外がスローされるのは、<see cref="IBindingTypeDef">バインディングタイプ定義</see>が
    /// <code>must</code>で自動バインディングの対象が見つからなかった時です。
    /// </para>
    /// </remarks>
    /// <seealso cref="IAutoBindingDef"/>
    /// <seealso cref="IBindingTypeDef"/>
    [Serializable]
    public class IllegalAutoBindingPropertyRuntimeException : SRuntimeException
    {
        private Type componentType;
        private string propertyName;

        public IllegalAutoBindingPropertyRuntimeException(Type componentType,
            string propertyName)
            : base("ESSR0080", new object[] { componentType.FullName, propertyName })
        {
            this.componentType = componentType;
            this.propertyName = propertyName;
        }

        /// <summary>
        /// 自動バインディングに失敗したコンポーネントのクラスのTypeを返します。
        /// </summary>
        /// <value>自動バインディングに失敗したコンポーネントのクラスのType</value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// 自動バインディング対象が見つからなかったプロパティまたはフィールドの名称を返します。
        /// </summary>
        /// <value>自動バインディングに失敗したプロパティまたはフィールドの名称</value>
        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}
