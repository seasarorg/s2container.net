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
using Seasar.Framework.Aop;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// コンポーネントに組み込むインタータイプを定義するインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// インタータイプ定義は、diconファイルにおける<code>&lt;interType&gt;</code>要素で指定されます。
    /// <code>&lt;interType&gt;</code>要素にはclass属性が含まれています。
    /// </para>
    /// <para>
    /// class属性には<see cref="Seasar.Framework.Aop.IInterType">インタータイプ</see>
    /// を実装したクラスを指定します。
    /// </para>
    /// <para>
    /// InterTypeは「静的な構造の変更」を実現します。 「静的な構造の変更」は下記のものを含みます。
    /// </para>
    /// <list type="bullet">
    /// <item><term>スーパークラスの変更</term></item>
    /// <item><term>実装インターフェースの追加</term></item>
    /// <item><term>フィールドの追加</term></item>
    /// <item><term>コンストラクタの追加</term></item>
    /// <item><term>メソッドの追加</term></item>
    /// </list>
    /// </remarks>
    public interface IInterTypeDef
    {
        /// <summary>
        /// インタータイプを返します。
        /// </summary>
        /// <value>インタータイプ</value>
        IInterType InterType { get; }
    }
}
