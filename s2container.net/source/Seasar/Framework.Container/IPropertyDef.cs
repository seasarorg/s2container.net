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
    /// コンポーネントのプロパティまたはフィールドにインジェクションする方法を定義するインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// プロパティ定義は、diconファイルにおける<code>&lt;property&gt;</code>要素で指定されます。
    /// <code>&lt;property&gt;</code>要素にはname属性とbindingType属性が含まれています。
    /// </para>
    /// <list type="bullet">
    /// <item><term>name属性はコンポーネントのプロパティ名またはフィールド名を指定します。</term></item>
    /// <item><term>bindingType属性はname属性にて指定されたプロパティまたはフィールドに、
    /// S2コンテナ内に格納されているコンポーネントをバインディングする際の挙動を指定します。</term></item>
    /// </list>
    /// <para>
    /// <code>&lt;property&gt;</code>要素の内容に指定された式またはコンポーネントが、
    /// <code>&lt;property&gt;</code>要素のname属性で指定されたプロパティまたはフィールドに設定されます。
    /// </para>
    /// <para>
    /// プロパティ定義が存在する場合のプロパティインジェクションまたはフィールドインジェクションは、
    /// diconファイルに記述されているプロパティ定義に従って行われます。
    /// プロパティ定義が存在しない場合、<see cref="IAutoBindingDef">自動バインディング定義</see>の
    /// 定義によって自動バインディングが行われる事があります。
    /// </para>
    /// </remarks>
    public interface IPropertyDef : IArgDef
    {
        /// <summary>
        /// インジェクション対象となるプロパティ名またはフィールド名を返します。
        /// </summary>
        /// <value>設定対象となるプロパティ名</value>
        string PropertyName { get; }

        /// <summary>
        /// バインディングタイプ定義を取得・設定します。
        /// </summary>
        /// <value>バインディングタイプ定義</value>
        IBindingTypeDef BindingTypeDef { get; set; }

        /// <summary>
        /// アクセスタイプ定義を取得・設定します。
        /// </summary>
        /// <value>アクセスタイプ定義</value>
        IAccessTypeDef AccessTypeDef { get; set; }
    }
}
