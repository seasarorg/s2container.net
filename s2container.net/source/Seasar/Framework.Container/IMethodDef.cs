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

namespace Seasar.Framework.Container
{
    /// <summary>
    /// メソッド・インジェクションを定義するためのインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// メソッド・インジェクションとは、任意のメソッドや式の呼び出しにより
    /// コンポーネントをインジェクションすることです。
    /// </para>
    /// <para>
    /// 例として、<code>addFoo(Foo)</code> メソッドを通じて <code>Foo</code>を
    /// インジェクションする場合に利用することができます。
    /// 引数のないメソッドや任意の式を呼び出すこともできます。
    /// </para>
    /// <para>
    /// コンポーネントが初期化されるときに実行されるinitMethodインジェクションと、
    /// コンテナの終了時に実行されるdesoryMethodインジェクションがあります。 
    /// DestroyMethodインジェクションが適用されるのは、
    /// コンポーネントのinstance要素が<code>singleton</code>の場合だけです。
    /// </para>
    /// </remarks>
    public interface IMethodDef : IArgDefAware
    {
        /// <summary>
        /// 実行するメソッドを返します。
        /// </summary>
        /// <value>実行するメソッド</value>
        MethodInfo Method { get; }

        /// <summary>
        /// メソッド名を返します。
        /// </summary>
        /// <value>メソッド名</value>
        string MethodName { get; }

        /// <summary>
        /// メソッド引数を返します。
        /// </summary>
        /// <value>メソッド引数</value>
        object[] Args { get; }

        /// <summary>
        /// 引数および式を評価するコンテキストとなるS2コンテナを取得・設定します。
        /// </summary>
        /// <value>引数および式を評価するコンテキストとなるS2コンテナ</value>
        IS2Container Container { get; set; }

        /// <summary>
        /// 実行される式を取得・設定します。
        /// </summary>
        /// <value>実行される式</value>
        IExpression Expression { get; set; }
    }
}
