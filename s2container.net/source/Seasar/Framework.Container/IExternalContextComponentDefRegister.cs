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

namespace Seasar.Framework.Container
{
    /// <summary>
    /// 外部コンテキストが提供するコンポーネント定義を、 S2コンテナに登録します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <code>IExternalContextComponentDefRegister</code>が外部コンテキストの
    /// <see cref="IComponentDef">コンポーネント定義</see>を登録することにより、
    /// <see cref="IExternalContext"/>インターフェースを通して、 
    /// 外部コンテキストのコンポーネントを取得できるようになります。
    /// </para>
    /// <para>
    /// コンポーネントを取得可能な外部コンテキストの種類については、
    /// <see cref="IExternalContext"/>インターフェースを参照して下さい。
    /// </para>
    /// </remarks>
    public interface IExternalContextComponentDefRegister
    {
        /// <summary>
        /// 指定されたS2コンテナに、 外部コンテキストのコンポーネント定義を登録します。
        /// </summary>
        /// <param name="container">S2コンテナ</param>
        void RegisterComponentDefs(IS2Container container);
    }
}
