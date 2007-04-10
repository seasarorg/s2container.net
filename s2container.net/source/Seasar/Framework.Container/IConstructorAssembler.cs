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
    /// コンストラクタ・インジェクションを実行してコンポーネントを組み立てます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IComponentDef">コンポーネント定義</see>
    /// に対して明示的にコンストラクタの引数が指定されなかった時の動作は、
    /// <see cref="IAutoBindingDef">自動バインディングタイプ定義</see>に基づきます。
    /// </para>
    /// </remarks>
    /// <seealso cref="Seasar.Framework.Container.Assembler.AbstractConstructorAssembler"/>
    /// <see cref="Seasar.Framework.Container.Assembler.AutoConstructorAssembler"/>
    /// <see cref="Seasar.Framework.Container.Assembler.DefaultConstructorConstructorAssembler"/>
    public interface IConstructorAssembler
    {
        /// <summary>
        /// コンストラクタ・インジェクションを実行して、 組み立てたコンポーネントを返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// また、<see cref="IComponentDef">コンポーネント定義</see>に
        /// <see cref="IExpression">式</see>が指定されていた場合、
        /// 式の評価結果をコンポーネントとして返します。
        /// </para>
        /// </remarks>
        /// <returns>コンストラクタ・インジェクション済みのコンポーネントのインスタンス</returns>
        /// <exception cref="Seasar.Framework.Beans.ConstructorNotFoundRuntimeException">
        /// 適切なコンストラクタが見つからなかった場合
        /// </exception>
        /// <exception cref="IllegalConstructorRuntimeException">
        /// コンストラクタの引数となるコンポーネントが見つからなかった場合
        /// </exception>
        /// <exception cref="ClassUnmatchRuntimeException">
        /// 組み立てたコンポーネントの型がコンポーネント定義のクラス指定に適合しなかった場合
        /// </exception>
        object assembler();
    }
}
