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
    /// 自動バインディングを適用する範囲を表す自動バインディング定義のインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>自動バインディング定義には、 以下のものがあります。</para>
    /// <list type="bullet">
    /// <item>
    /// <term><code>auto</code></term>
    /// <description>コンストラクタとプロパティの両方で、 自動バインディングを適用します。</description>
    /// </item>
    /// <item>
    /// <term><code>semiauto</code></term>
    /// <description>明示的に指定したプロパティに対してのみ自動バインディングを適用します。</description>
    /// </item>
    /// <item>
    /// <term><code>constructor</code></term>
    /// <description>コンストラクタの自動バインディングのみ適用します。</description>
    /// </item>
    /// <item>
    /// <term><code>property</code></term>
    /// <description>プロパティの自動バインディングのみ適用します。</description>
    /// </item>
    /// <item>
    /// <term><code>none</code></term>
    /// <description>すべての自動バインディングを適用しません。</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <seealso cref="AutoBindingDefConstants"/>
    public interface IAutoBindingDef
    {
        /// <summary>
        /// 自動バインディング定義名を返します。
        /// </summary>
        /// <value>自動バインディング定義名</value>
        string Name { get; }

        /// <summary>
        /// 自動バインディング定義に基づき、 <code>componentDef</code>に対する
        /// <see cref="IConstructorAssembler"/>を返します。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <returns>自動バインディングの範囲が設定された<see cref="IConstructorAssembler"/></returns>
        IConstructorAssembler CreateConstructorAssembler(IComponentDef componentDef);

        /// <summary>
        /// 自動バインディング定義に基づき、 <code>componentDef</code>に対する
        /// <see cref="IPropertyAssembler"/>を返します。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <returns>自動バインディングの範囲が設定された<see cref="IPropertyAssembler"/></returns>
        IPropertyAssembler CreatePropertyAssembler(IComponentDef componentDef);
    }
}
