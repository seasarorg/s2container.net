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
using System.Reflection;
using Seasar.Framework.Beans;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// コンポーネントをインジェクションする時の動作を表すバインディングタイプを定義するインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>バインディングタイプ定義には、 以下のものがあります。</para>
    /// <list type="bullet">
    /// <item>
    /// <term><code>must</code></term>
    /// <description>自動バインディングが適用できなかった場合、 例外が発生します。</description>
    /// </item>
    /// <item>
    /// <term><code>should</code></term>
    /// <description>自動バインディングが適用できなかった場合、 警告を通知します。</description>
    /// </item>
    /// <item>
    /// <term><code>may</code></term>
    /// <description>自動バインディングが適用できなかった場合でも何もおきません。</description>
    /// </item>
    /// <item>
    /// <term><code>none</code></term>
    /// <description><see cref="IAutoBindingDef">自動バインディング定義</see>が
    /// <code>auto</code>や<code>property</code>の場合でも自動バインディングを適用しません。
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public interface IBindingTypeDef
    {
        /// <summary>
        /// バインディングタイプ定義名を返します。
        /// </summary>
        /// <value>バインディングタイプ定義名</value>
        string Name { get; }

        /// <summary>
        /// バインディングタイプ定義に基づいて、 <code>component</code>に対して
        /// S2コンテナ上のコンポーネントをプロパティ経由でインジェクションします。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <param name="propertyDef">プロパティに対する設定方法や設定値の定義</param>
        /// <param name="propertyDesc">対象となるコンポーネントのプロパティ情報</param>
        /// <param name="component">対象となるコンポーネントのインスタンス</param>
        void Bind(IComponentDef componentDef, IPropertyDef propertyDef,
            IPropertyDesc propertyDesc, object component);

        /// <summary>
        /// バインディングタイプ定義に基づいて、 <code>component</code>に対して
        /// S2コンテナ上のコンポーネントをフィールドに直接インジェクションします。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <param name="propertyDef">プロパティに対する設定方法や設定値の定義</param>
        /// <param name="field">対象となるコンポーネントのフィールド情報</param>
        /// <param name="component">対象となるコンポーネントのインスタンス</param>
        /// <exception cref="Seasar.Framework.Beans.IllegalPropertyRuntimeException">
        /// <code>propertyDef</code>に指定されたコンポーネントが取得できなかった場合
        /// </exception>
        /// <exception cref="Seasar.Framework.Exceptions.IllegalAccessRuntimeException">
        /// 対象となるコンポーネントのフィールドが<code>private</code>などでアクセスできなかった場合
        /// </exception>
        void Bind(IComponentDef componentDef, IPropertyDef propertyDef, FieldInfo field,
            object component);
    }
}
