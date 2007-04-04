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
    /// 取得しようとしたコンポーネントがS2コンテナ上に見つからなかった場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// コンポーネントの検索には、 コンポーネントキー(キーオブジェクト)として、 
    /// クラス(インターフェース)またはコンポーネント名が使用できますが、
    /// どちらの場合でもコンポーネントが見つからなかった場合には、 この例外がスローされます。
    /// </para>
    /// </remarks>
    [Serializable]
    public class ComponentNotFoundRuntimeException : SRuntimeException
    {
        private object componentKey;

        /// <summary>
        /// コンポーネントの検索に用いたコンポーネントキーを指定して、
        /// <code>ComponentNotFoundRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="componentKey">コンポーネントキー</param>
        public ComponentNotFoundRuntimeException(object componentKey)
            : base("ESSR0046", new object[] { componentKey })
        {
            this.componentKey = componentKey;
        }

        /// <summary>
        /// コンポーネントの検索に用いたコンポーネントキー
        /// </summary>
        public object ComponentKey
        {
            get { return componentKey; }
        }
    }
}
