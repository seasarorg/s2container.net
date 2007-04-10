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
    /// プロパティ・インジェクションやフィールド・インジェクションを実行してコンポーネントを組み立てます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// インジェクションの実行は、<see cref="IPropertyDef">プロパティ定義</see>に基づいて行います。 
    /// プロパティ定義が指定されていない場合の動作は、
    /// <see cref="IAutoBindingDef">自動バインディング</see>に基づきます。
    /// </para>
    /// <para>
    /// また、<see cref="IComponentDef">コンポーネント定義</see>の
    /// <code>externalBinding</code>属性が<code>true</code>の場合、
    /// <see cref="IExternalContext"/>の保持している値もバインディングの対象とします。
    /// </para>
    /// </remarks>
    /// <seealso cref="IPropertyDef"/>
    /// <seealso cref="IAutoBindingDef"/>
    /// <seealso cref="IExternalContext"/>
    public interface IPropertyAssembler
    {
        /// <summary>
        /// 指定された<code>component</code>に対して、
        /// プロパティ・インジェクションやフィールド・インジェクションを実行します。
        /// コンポーネント定義の<code>externalBinding</code>属性が<code>true</code>にも関わらず、
        /// <see cref="IExternalContext"/>がS2コンテナに設定されていない場合には、
        /// EmptyRuntimeExceptionをスローします。
        /// </summary>
        /// <param name="component">S2コンテナ上のコンポーネントがセットされる対象</param>
        /// <exception cref="Seasar.Framework.Beans.IllegalPropertyRuntimeException">
        /// プロパティが見つからないなどの理由でインジェクションに失敗した場合
        /// </exception>
        /// <exception cref="Seasar.Framework.Exceptions.EmptyRuntimeException">
        /// ExternalContextがS2コンテナに設定されていない場合
        /// </exception>
        void Assemble(object component);
    }
}
