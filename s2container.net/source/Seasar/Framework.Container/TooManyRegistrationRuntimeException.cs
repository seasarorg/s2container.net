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
using System.Text;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// 1つのキーに複数のコンポーネントが登録されていた場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// S2コンテナからコンポーネントを取得しようとした際に、 指定したキー(コンポーネントのクラス、 インターフェース、
    /// あるいは名前)に該当するコンポーネント定義が複数存在した場合、 この例外が発生します。
    /// </para>
    /// </remarks>
    [Serializable]
    public sealed class TooManyRegistrationRuntimeException : SRuntimeException
    {
        private object key;
        private Type[] componentTypes;

        /// <summary>
        /// <code>TooManyRegistrationRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="key">コンポーネントを取得しようとした際に使用したキー</param>
        /// <param name="componentTypes">1つのキーに登録された複数コンポーネントのTypeの配列</param>
        public TooManyRegistrationRuntimeException(object key, Type[] componentTypes)
            : base("ESSR0045", new object[] { key, GetTypeNames(componentTypes) })
        {
            this.key = key;
            this.componentTypes = componentTypes;
        }

        /// <summary>
        /// コンポーネントを取得しようとした際に使用したキーを返します。
        /// </summary>
        /// <value>コンポーネントを取得するためのキー</value>
        public object Key
        {
            get { return key; }
        }

        /// <summary>
        /// 1つのキーに登録された複数コンポーネントのTypeの配列を返します。
        /// </summary>
        /// <value>コンポーネントのTypeの配列</value>
        public Type[] ComponentTypes
        {
            get { return componentTypes; }
        }

        private static string GetTypeNames(Type[] componentTypes)
        {
            StringBuilder buf = new StringBuilder(255);

            foreach (Type componentType in componentTypes)
            {
                if (componentType != null)
                {
                    buf.Append(componentType.FullName);
                }
                else
                {
                    buf.Append("<unknown>");
                }

                buf.Append(", ");
            }

            buf.Length -= 2;

            return buf.ToString();
        }
    }
}
